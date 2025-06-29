using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class PrivacyPolicyButton : MonoBehaviour
{
    // Açmak istediðin web sitesi URL'si
    public string url = "https://yenisporbilgi.blogspot.com/2024/11/privacy-policy.html";

    public void OpenPrivacyPolicy()
    {
        Application.OpenURL(url);
    }
}
