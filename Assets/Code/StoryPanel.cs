using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class StoryPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panelObject;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private TextMeshProUGUI continuePrompt; // "Press E to continue..."
    
    [Header("Settings")]
    [SerializeField] private float verticalOffset = 100f; // Offset über dem Charakter
    
    // Nutze die selbe Interact-Action wie im PlayerController
    private InputAction continueAction;
    
    private Transform followTarget; // Der Character
    private Camera mainCamera;
    
    private List<string> pages = new List<string>();
    private int currentPageIndex = 0;
    private bool isShowingStory = false;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        
        if (panelObject != null)
            panelObject.SetActive(false);
            
        if (continuePrompt != null)
            continuePrompt.gameObject.SetActive(false);
        
        // Interact Action aus dem Input System holen (selbe wie im PlayerController)
        continueAction = InputSystem.actions.FindAction("Interact");
    }
    
    private void Update()
    {
        if (!isShowingStory) return;
        
        // Panel über dem Character positionieren
        if (followTarget != null)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(followTarget.position);
            screenPos.y += verticalOffset;
            panelObject.transform.position = screenPos;
        }
        
        // Weiter-Taste drücken
        if (continueAction != null && continueAction.WasPressedThisFrame())
        {
            NextPage();
        }
    }
    
    /// <summary>
    /// Zeigt eine Story an, die automatisch in Seiten aufgeteilt wird
    /// </summary>
    public void ShowStory(string fullText, Transform characterTransform)
    {
        followTarget = characterTransform;
        
        // Text in Seiten aufteilen basierend auf der TextMeshPro-Größe
        pages = SplitTextIntoPages(fullText);
        currentPageIndex = 0;
        
        isShowingStory = true;
        panelObject.SetActive(true);
        
        // Erste Seite anzeigen
        DisplayCurrentPage();
        
        // NICHT Time.timeScale nutzen - interferiert mit GameJam State-System
        // Bei Bedarf im PlayerController ein "isReadingStory" Flag setzen
    }
    
    /// <summary>
    /// Versteckt das Story Panel
    /// </summary>
    public void HideStory()
    {
        isShowingStory = false;
        panelObject.SetActive(false);
        followTarget = null;
        pages.Clear();
    }
    
    private void NextPage()
    {
        currentPageIndex++;
        
        if (currentPageIndex >= pages.Count)
        {
            // Alle Seiten gelesen - Panel schließen
            HideStory();
        }
        else
        {
            // Nächste Seite anzeigen
            DisplayCurrentPage();
        }
    }
    
    private void DisplayCurrentPage()
    {
        if (pages.Count == 0) return;
        
        storyText.text = pages[currentPageIndex];
        
        // "Continue" Prompt nur anzeigen wenn es weitere Seiten gibt
        bool hasMorePages = currentPageIndex < pages.Count - 1;
        
        if (continuePrompt != null)
        {
            continuePrompt.gameObject.SetActive(true);
            continuePrompt.text = hasMorePages 
                ? "Press E to continue..." 
                : "Press E to close";
        }
    }
    
    /// <summary>
    /// Teilt einen langen Text in mehrere Seiten auf, basierend auf der verfügbaren Höhe
    /// </summary>
    private List<string> SplitTextIntoPages(string fullText)
    {
        List<string> resultPages = new List<string>();
        
        if (string.IsNullOrEmpty(fullText))
            return resultPages;
        
        // Temporarily set the full text to measure it
        storyText.text = fullText;
        storyText.ForceMeshUpdate();
        
        // Get the preferred height for the full text
        float fullTextHeight = storyText.preferredHeight;
        float availableHeight = ((RectTransform)storyText.transform).rect.height;
        
        // If text fits in one page, return it as is
        if (fullTextHeight <= availableHeight)
        {
            resultPages.Add(fullText);
            return resultPages;
        }
        
        // Split text into words
        string[] words = fullText.Split(' ');
        string currentPage = "";
        
        foreach (string word in words)
        {
            string testPage = string.IsNullOrEmpty(currentPage) 
                ? word 
                : currentPage + " " + word;
            
            // Test if this fits
            storyText.text = testPage;
            storyText.ForceMeshUpdate();
            
            if (storyText.preferredHeight > availableHeight)
            {
                // Current page is full, start a new one
                if (!string.IsNullOrEmpty(currentPage))
                {
                    resultPages.Add(currentPage.Trim());
                }
                currentPage = word;
            }
            else
            {
                currentPage = testPage;
            }
        }
        
        // Add the last page
        if (!string.IsNullOrEmpty(currentPage))
        {
            resultPages.Add(currentPage.Trim());
        }
        
        return resultPages;
    }
}