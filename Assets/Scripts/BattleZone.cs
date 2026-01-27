using UnityEngine;
using System.Collections.Generic;
using TMPro; 
using System.Collections; 

public class BattleZone : MonoBehaviour
{
    [Header("Setup")]
    public GameObject[] invisibleWalls;
    public List<GameObject> enemiesToKill;

    [Header("UI Messages")]
    public TextMeshProUGUI stageTextUI; // ใช้ Text ตัวเดิมกลางจอ
    public string stageName = "STAGE 1-1"; 
    public string clearMessage = "STAGE CLEAR!"; // 👈 เพิ่มคำพูดตอนจบ
    public float showDuration = 3f; 

    private bool isActivated = false;
    private bool isCleared = false; // กันบั๊กเรียกซ้ำ

    void Start()
    {
        // ปิดกำแพงตอนเริ่ม
        foreach (var wall in invisibleWalls)
        {
            if(wall != null) wall.SetActive(false);
        }

        if (stageTextUI != null) stageTextUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isActivated || isCleared) return; // ถ้าจบแล้วไม่ต้องเช็คต่อ

        enemiesToKill.RemoveAll(item => item == null);

        if (enemiesToKill.Count == 0)
        {
            UnlockZone(); // เรียกฟังก์ชันจบด่าน
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            if (enemiesToKill.Count > 0)
            {
                LockZone();
            }
        }
    }

    // ================== เริ่มสู้ ==================
    void LockZone()
    {
        isActivated = true;

        // กางอาณาเขต
        foreach (var wall in invisibleWalls)
        {
            if(wall != null) wall.SetActive(true);
        }

        // โชว์ชื่อด่าน
        if (stageTextUI != null)
        {
            stageTextUI.text = stageName; 
            stageTextUI.gameObject.SetActive(true); 

            // หยุด Coroutine เก่า (เผื่อมีค้างอยู่) แล้วเริ่มอันใหม่
            StopAllCoroutines();
            StartCoroutine(HideTextRoutine()); 
        }
    }

    // ================== จบด่าน ==================
    void UnlockZone()
    {
        isCleared = true; // ล็อกไว้ไม่ให้เรียกซ้ำ

        // ปลดกำแพงทันที (ให้เดินได้เลยไม่ต้องรอข้อความจบ)
        foreach (var wall in invisibleWalls)
        {
            if(wall != null) wall.SetActive(false);
        }

        // โชว์คำว่า STAGE CLEAR!
        if (stageTextUI != null)
        {
            stageTextUI.text = clearMessage; // เปลี่ยนข้อความ
            stageTextUI.gameObject.SetActive(true);

            // เรียก Coroutine พิเศษสำหรับจบด่าน
            StartCoroutine(ClearSequenceRoutine());
        }
        else
        {
            // ถ้าไม่มี UI ให้ทำลายตัวเองเลย
            Destroy(gameObject);
        }

        Debug.Log("🔓 ZONE CLEAR!");
    }

    // ตัวนับเวลาสำหรับตอนเริ่ม (แค่ซ่อนข้อความ)
    IEnumerator HideTextRoutine()
    {
        yield return new WaitForSeconds(showDuration);
        if (stageTextUI != null) stageTextUI.gameObject.SetActive(false);
    }

    // ตัวนับเวลาสำหรับตอนจบ (ซ่อนข้อความ -> ทำลายตัวเอง)
    IEnumerator ClearSequenceRoutine()
    {
        yield return new WaitForSeconds(showDuration); // โชว์ค้างไว้ 3 วิ

        if (stageTextUI != null) stageTextUI.gameObject.SetActive(false); // ซ่อนข้อความ

        Destroy(gameObject); // ทำลายโซนทิ้ง (ภารกิจเสร็จสิ้น)
    }
}