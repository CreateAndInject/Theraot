﻿//
// LazyInitializerTest.cs
//
// Authors:
//       Marek Safar (marek.safar@gmail.com)
//
// Copyright 2011 Xamarin, Inc (http://www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#define NET_4_0
#if NET_4_0

using NUnit.Framework;
using System;
using System.Threading;

namespace MonoTests.System.Threading
{
    [TestFixture]
    public class LazyInitializerTest
    {
        private class C
        {
            public C()
            {
                ++Counter;
            }

            public int Counter { get; private set; }
        }

        [Test]
        public void EnsureInitialized_Simple()
        {
            C c = null;
            LazyInitializer.EnsureInitialized(ref c);

            Assert.IsNotNull(c, "#1");
            Assert.AreEqual(1, c.Counter, "#2");
        }

        [Test]
        public void EnsureInitialized_FactoryIsNull()
        {
            C c = null;
            try
            {
                LazyInitializer.EnsureInitialized(ref c, () => null);
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                Theraot.No.Op(ex);
            }
        }

        [Test]
        public void EnsureInitialized_NullLock()
        {
            C c = null;
            var init = false;
            object syncLock = null;
            c = LazyInitializer.EnsureInitialized(ref c, ref init, ref syncLock);

            Assert.IsNotNull(c, "#1");
            Assert.AreEqual(1, c.Counter, "#2");
            Assert.IsTrue(init, "#3");
            Assert.IsNotNull(syncLock, "#4");

            var oldLock = syncLock;
            var d = LazyInitializer.EnsureInitialized(ref c, ref init, ref syncLock);

            Assert.AreEqual(c, d, "#11");
            Assert.AreEqual(1, c.Counter, "#12");
            Assert.IsTrue(init, "#13");
            Assert.AreEqual(oldLock, syncLock, "#14");
        }
    }
}

#endif