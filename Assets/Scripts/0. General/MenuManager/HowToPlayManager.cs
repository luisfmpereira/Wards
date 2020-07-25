using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayManager : MonoBehaviour
{



    [SerializeField]
    private GameObject[] pages;
    [SerializeField]
    private int currentPage;

    [SerializeField]
    private GameObject arrowRight;
    [SerializeField]
    private GameObject arrowLeft;
    private Image arrowRightImage;
    private Image arrowLeftImage;
    private AudioManager AudioManager;



    private bool isCooldown;
    private float currentCooldown;
    private float inputCooldown = 0.5f;
    private float inputValue;


    private void Awake()
    {
        AudioManager = AudioManager.Instance;
        arrowRightImage = arrowRight.GetComponent<Image>();
        arrowLeftImage = arrowLeft.GetComponent<Image>();
    }

    private void OnEnable()
    {
        ShowFirstPage();
        currentPage = 0;
        CheckArrowsToShow();
    }

    private void Update()
    {
        inputValue = Input.GetAxis("Horizontal");

        if (isCooldown)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0)
                isCooldown = false;
        }
        else
        {
            if (inputValue > 0)
                ChangePageToRight();

            if (inputValue < 0)
                ChangePageToLeft();
        }

    }

    void CheckArrowsToShow()
    {
        if (currentPage == pages.Length - 1)
        {
            arrowRight.SetActive(false);
        }

        else if (currentPage == 0)
            arrowLeft.SetActive(false);
        else
        {
            arrowRight.SetActive(true);
            arrowLeft.SetActive(true);
        }

    }

    void ChangePageToRight()
    {
        if (currentPage < pages.Length - 1)
        {
            AudioManager.PlaySound("ButtonClick");

            StartCoroutine(BlinkImage(arrowRightImage));

            DisablePage(currentPage);
            currentPage++;
            EnablePage(currentPage);
            CheckArrowsToShow();
        }
    }

    void ChangePageToLeft()
    {
        if (currentPage > 0)
        {
            AudioManager.PlaySound("ButtonClick");

            StartCoroutine(BlinkImage(arrowLeftImage));

            DisablePage(currentPage);
            currentPage--;
            EnablePage(currentPage);
            CheckArrowsToShow();
        }
    }

    void ShowFirstPage()
    {
        pages[0].SetActive(true);
        pages[1].SetActive(false);
        pages[2].SetActive(false);
        pages[3].SetActive(false);
        arrowRight.SetActive(true);
        arrowLeft.SetActive(false);
        
    }

    void DisablePage(int pageNumber)
    {
        pages[pageNumber].SetActive(false);
        ResetCooldown();

    }
    void EnablePage(int pageNumber)
    {
        pages[pageNumber].SetActive(true);
        ResetCooldown();
    }

    void ResetCooldown()
    {
        isCooldown = true;
        currentCooldown = inputCooldown;
    }

    IEnumerator BlinkImage(Image image)
    {
        image.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        image.color = Color.white;
        yield return new WaitForSeconds(0.1f);
    }





}
