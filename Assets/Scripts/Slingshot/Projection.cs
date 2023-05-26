using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour
{
    // public void SimulateTrajectory(Throwable throwable, Vector3 pos, Vector3 velocity)
    // {
    //     var ghostObj = Instantiate(throwable, pos, Quaternion.identity);
    //     // SceneManager.MoveGameObjectToScene(ghostObj.gameObject, _simulationScene);

    //     ghostObj.Init(velocity, true);

    //     _line.positionCount = _maxPhysicsFrameIterations;

    //     for (var i = 0; i < _maxPhysicsFrameIterations; i++)
    //     {
    //         _physicsScene.Simulate(Time.fixedDeltaTime);
    //         _line.SetPosition(i, ghostObj.transform.position);
    //     }

    //     Destroy(ghostObj.gameObject);
    // }
}
