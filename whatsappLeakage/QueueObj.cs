using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace whatsappLeakage
{
    class QueueObj
    {

        private Queue<TestProp> qt;
        private Queue<Driver> drt;
        private Driver dd1;
        private Driver dd2;

        public QueueObj()
        {
            qt = new Queue<TestProp>();
            drt = new Queue<Driver>();
            dd1 = new Driver(1);
            dd2 = new Driver(2);

            drt.Enqueue(dd1);
            drt.Enqueue(dd2);


        }
        public void AddQ(string N,string T,string TerID)
        {
            lock (this)
            {
                TestProp prop = new TestProp(N, T, TerID);
                qt.Enqueue(prop);
            }
        }
        public TestProp GetQ()
        {
            lock (this)
            {
                return qt.Dequeue();
            }
        }

        public Driver GetDriver()
        {
            lock (this)
            {
                return drt.Dequeue();
            }
        }
        public TestProp GetTest()
        {
            lock (this)
            {
                return qt.Dequeue();
            }
        }
        public int GetDriverCount()
        {
            lock (this)
            {
                return drt.Count;
            }
        }
        public int GetTestCount()
        {
            lock (this)
            {
                return qt.Count;
            }
        }
        public void releasDriver(Driver dd)
        {
            lock (this)
            {
                drt.Enqueue(dd);
            }
        }
    }
}
