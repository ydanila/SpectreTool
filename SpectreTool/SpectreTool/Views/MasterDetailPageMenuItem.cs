using System;

namespace SpectreTool.Views
{
	public class MasterDetailPageMenuItem
	{
		public ViewType View { get; set; }

		public string Title { get; set; }

		public Type TargetType { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType()) return false;

			var item = (MasterDetailPageMenuItem) obj;

			return View == item.View && Title.Equals(item.Title) && TargetType.Equals(item.TargetType);
		}

		public override int GetHashCode()
		{
			var title = string.IsNullOrEmpty(Title) ? string.Empty : Title;
			var type = TargetType == null ? typeof(object) : TargetType;

			return View.GetHashCode() ^ title.GetHashCode() ^ type.GetHashCode() ^ base.GetHashCode();
		}
	}
}