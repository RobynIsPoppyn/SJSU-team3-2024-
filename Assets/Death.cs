using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
      void Die(){
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
