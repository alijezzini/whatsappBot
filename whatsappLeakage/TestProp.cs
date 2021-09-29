using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace whatsappLeakage
{
    class TestProp
    {
        private String Number;
        private String Text;
        private String TerID;
        public TestProp(String Number,String Text,String TerID)
        {
            this.Number = Number;
            this.Text = Text;
            this.TerID = TerID;

        }
        public String GetNumber()
        {
            return Number;
        }
        public String GetText()
        {
            return Text;
        }
        public String GetTerID()
        {
            return TerID;
        }

    }
}
