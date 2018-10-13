using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace CustomXF.Lib
{
	public class SingleColumnLayout : Layout<View>
	{
		private readonly Dictionary<Size, LayoutData> layoutDataCache = new Dictionary<Size, LayoutData>();

		public static readonly BindableProperty MaxWidthProperty = BindableProperty.Create(
			"MaxWidth",
			typeof(double),
			typeof(SingleColumnLayout),
			double.PositiveInfinity,
			propertyChanged: (bindable, oldvalue, newvalue) => { ((SingleColumnLayout) bindable).InvalidateLayout(); });

		public double MaxWidth
		{
			set => SetValue(MaxWidthProperty, value);
			get => (double) GetValue(MaxWidthProperty);
		}

		protected override void OnChildAdded(Element child)
		{
			base.OnChildAdded(child);
			if (Children.Count > 1)
			{
				//	if it already has child - we can't add more
				throw new InvalidOperationException("Layout can have only one child");
			}
		}

		protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
		{
			var layoutData = GetLayoutData(widthConstraint, heightConstraint);
			if (layoutData.VisibleChildCount == 0)
			{
				return new SizeRequest();
			}

			var totalSize = new Size(layoutData.ColumnSize.Width + layoutData.Spacing * 2, layoutData.ColumnSize.Height);
			return new SizeRequest(totalSize);
		}

		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			var layoutData = GetLayoutData(width, height);
			if (layoutData.VisibleChildCount == 0)
			{
				return;
			}

			var child = Children.FirstOrDefault();
			if (child == null || !child.IsVisible)
			{
				return;
			}

			LayoutChildIntoBoundingRegion(child, new Rectangle(new Point(x + layoutData.Spacing, y), layoutData.ColumnSize));
		}

		private LayoutData GetLayoutData(double width, double height)
		{
			var size = new Size(width, height);

			// Check if cached information is available.
			if (layoutDataCache.ContainsKey(size))
			{
				return layoutDataCache[size];
			}

			var resultSize = new Size();
			var layoutData = new LayoutData();

			var child = Children.FirstOrDefault();
			if(child == null || !child.IsVisible)
			{
				return layoutData;
			}

			var childSizeRequest = child.Measure(double.PositiveInfinity, double.PositiveInfinity);
			if(width > childSizeRequest.Minimum.Width)
			{
				resultSize.Width = Math.Min(MaxWidth, Math.Min(width, childSizeRequest.Request.Width));
			}
			else
			{
				resultSize.Width = childSizeRequest.Minimum.Width;
			}

			//	if result size not equal expected by control - count again
			if (resultSize.Width != childSizeRequest.Request.Width)
			{
				childSizeRequest = child.Measure(resultSize.Width, double.PositiveInfinity);
			}

			if (height > childSizeRequest.Minimum.Height)
			{
				resultSize.Height = Math.Min(height, childSizeRequest.Request.Height);
			}
			else
			{
				resultSize.Height = childSizeRequest.Minimum.Height;
			}

			var spacingWidth = Math.Min(width, MaxWidth);
			var spacing = (spacingWidth - resultSize.Width) / 2;

			layoutData = new LayoutData(resultSize, spacing, 1);

			layoutDataCache.Add(size, layoutData);
			return layoutData;
		}

		protected override void InvalidateLayout()
		{
			base.InvalidateLayout();
			layoutDataCache.Clear();
		}

		protected override void OnChildMeasureInvalidated()
		{
			base.OnChildMeasureInvalidated();
			layoutDataCache.Clear();
		}

		private struct LayoutData
		{
			public Size ColumnSize { get; }

			public double Spacing { get; }

			public int VisibleChildCount { get; }

			public LayoutData(Size columnSize = new Size(), double spacing = 0, int visibleChildCount = 0)
			{
				ColumnSize = columnSize;
				Spacing = spacing;
				VisibleChildCount = visibleChildCount;
			}
		}
	}
}