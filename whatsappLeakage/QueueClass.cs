
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace whatsappLeakage
{
    class QueueClass
    {
        private QueueObj obj;
        public QueueClass(QueueObj obj)
        {
            this.obj = obj;

 
        }
        public void Run()
        {

            while (true)
                {
                int tests = obj.GetTestCount();
                int drivers = obj.GetDriverCount();
                
                if (tests > 0 && drivers>0)
                {
                    Driver driver = obj.GetDriver();
                    TestProp prop = obj.GetQ();
                    RunTest test = new RunTest(obj,prop.GetNumber(),prop.GetText(),prop.GetTerID(),driver);
                    Thread NewTest = new Thread(new ThreadStart(test.run));
                    NewTest.Start();
                }
                Thread.Sleep(10);
                }


        }
    }
}
