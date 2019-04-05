using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedDirt
{
    public class RedDirtGame : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void BindReaderToRegistry(Magnets.MagneticGestureReader reader)
        {
            if (reader.useTrainingSetFile)
            {
                if (reader.trainingSetFile == null)
                {
                    reader.trainingSetFile = Registry.Instance.trainingSet;
                }
            }
        }
    }
}