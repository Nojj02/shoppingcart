using System;
using System.Collections.Generic;
using System.Text;
using ShoppingCartApi.Model;
using ShoppingCartSharedKernel;
using Xunit;

namespace ShoppingCartApi.Tests.Model
{
    public class PercentTest
    {
        public class Equal : PercentTest
        {
            [Fact]
            public void ValueEquality()
            {
                var percent = new Percent(30);
                Assert.Equal(new Percent(30), percent);
            }
        }
    }
}
