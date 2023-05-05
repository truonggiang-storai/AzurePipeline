namespace UnitTest
{
    public class FiboUnitTest
    {
        [Fact]
        public void PassingTest1()
        {
            Assert.Equal(0, Fibo(1));
        }


        [Fact]
        public void PassingTest2()
        {
            Assert.Equal(1, Fibo(2));
        }


        [Fact]
        public void PassingTest3()
        {
            Assert.Equal(1, Fibo(3));
        }


        [Fact]
        public void PassingTest4()
        {
            Assert.Equal(144, Fibo(13));
        }

        [Fact]
        public void ExceptionTest1()
        {
            Assert.Throws<ArgumentException>(() => Fibo(0));
        }

        [Fact]
        public void ExceptionTest2()
        {
            Assert.Throws<ArgumentException>(() => Fibo(-1));
        }

        private int Fibo(int x)
        {
            if (x <= 0)
                throw new ArgumentException("Invalid argument");
            if (x == 1)
                return 0;
            if (x == 2) 
                return 1;
            else 
                return Fibo(x - 1) + Fibo(x - 2); 
        }
    }
}