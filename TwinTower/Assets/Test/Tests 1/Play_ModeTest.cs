using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TwinTower;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.TestTools;

public class Play_ModeTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void NewTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator UI_MainScreen_EnterCursor_Test()
    {
        GameObject go = ResourceManager.Instance.Instantiate($"UI/UI_MainScene");
        UI_MainScene ui = Util.GetOrAddComponent<UI_MainScene>(go);

        int k = ui.Test();
        yield return null;
        
        Assert.AreEqual(k, 3);
    }
}
