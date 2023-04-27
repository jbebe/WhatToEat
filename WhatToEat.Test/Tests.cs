using WhatToEat.App.Common;
using WhatToEat.App.Storage.Model;

namespace WhatToEat.Test
{
    public class Tests
    {
        [Fact]
        public void Test1()
        {
            var id1 = new Id<User>();
            var id2 = new Id<User>();
            var dict = new Dictionary<Id<User>, int>
            {
                [id1] = 1,
                [id2] = 2,
            };

            Assert.Equal(1, dict[id1]);
            Assert.Equal(2, dict[id2]);
        }
    }
}