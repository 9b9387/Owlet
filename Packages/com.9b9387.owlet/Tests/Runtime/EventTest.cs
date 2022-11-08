using NUnit.Framework;
using UnityEngine;

namespace Owlet.Tests
{
    public class EventTest
    {
        [Test]
        public void EventSubscribeTest()
        {
            EventCenter.UnsubscribeAll();

            UnityEngine.Events.UnityAction<object[]> OnEventAction = (args)=> {
                
            };
            EventCenter.Subscribe(1, OnEventAction);

            EventCenter.Subscribe(2, (p) =>
            {
                Debug.Log($"222 {p}");
            });

            EventCenter.Trigger(1, 1, "2");
            EventCenter.Trigger(2, "2");
            EventCenter.Trigger(1, null);

            EventCenter.Unsubscribe(1, OnEventAction);
            EventCenter.Trigger(1, 1, "2");

            EventCenter.UnsubscribeAll();
            EventCenter.Trigger(1);
            EventCenter.Trigger(2);

            Assert.Pass();
        }
    }
}
