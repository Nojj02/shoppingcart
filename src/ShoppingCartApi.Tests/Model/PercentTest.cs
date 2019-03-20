using System;
using System.Collections.Generic;
using System.Text;
using ShoppingCartApi.Model;
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
                var percent = new Percentage(30);
                Assert.Equal(new Percentage(30), percent);
            }
        }
    }
}
