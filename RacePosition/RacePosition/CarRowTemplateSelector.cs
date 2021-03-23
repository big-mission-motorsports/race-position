using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BigMission.RacePosition
{
    class CarRowTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SmallVertical { get; set; }
        public DataTemplate SmallHorz { get; set; }
        public DataTemplate Large { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var lv = container as ListView;
            if (lv != null)
            {
                var isPortrait = lv.Height > lv.Width;

                if (isPortrait && lv.Width < 600)
                {
                    lv.RowHeight = (int)(lv.Height / 3);
                    return SmallVertical;
                }
                else if (!isPortrait && lv.Height < 230)
                {
                    lv.RowHeight = (int)(lv.Height / 3);
                    return SmallHorz;
                }
            }
            return Large;
        }
    }
}

