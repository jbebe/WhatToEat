using WhatToEat.App.Common;
using WhatToEat.App.Storage.Model;

namespace WhatToEat.Test
{
    public class Tests
    {
        [Fact]
        public void Test1()
        {
            var id1a = new Id<User>("a");
			var id1b = new Id<User>("a");
			var id2 = new Id<User>("b");
            var dict = new Dictionary<Id<User>, int>();
            dict[id1a] = 1;
			dict[id1b] = 2;
			dict[id2] = 3;

            Assert.Equal(2, dict[id1a]);
			Assert.Equal(2, dict[id1b]);
			Assert.Equal(3, dict[id2]);
        }
    }
}