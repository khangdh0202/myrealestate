using System;
using TriLibCore;
using UnityEngine;

public class GCHelperManager : MonoBehaviour
{
    // Thời gian giữa mỗi lần gọi RegisterLoading() (ví dụ: 5 phút)
    public float interval = 30f;

    // Biến đếm thời gian
    private float timer = 0f;

    // Update is called once per frame
    /*void Update()
    {
        // Cập nhật biến đếm thời gian
        timer += Time.deltaTime;

        // Kiểm tra nếu đã đủ thời gian để gọi RegisterLoading()
        if (timer >= interval)
        {
            Debug.Log("Time to clear the trash");

            // Gọi RegisterLoading() và đặt lại biến đếm thời gian
            GC.Collect();
            timer = 0f;
        }
    }*/
}
