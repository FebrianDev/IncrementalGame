using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
    public Button resourceButton;
    public Image resourceImage;

    [SerializeField] private Text resourceDescription;
    [SerializeField] private Text resourceUpgradeCost;
    [SerializeField] private Text resourceUnlockCost;

    private ResourceConfig _config;

    private int _level = 1;
    
    public bool IsUnlocked { get; private set; }

    private void Start()
    {
        resourceButton.onClick.AddListener(() =>
        {
            if (IsUnlocked)
                UpgradeLevel();
            else
                UnlockResource();
        });
    }

    private void UnlockResource()
    {
        double unlockCost = GetUnlockCost ();
        if (GameManager.Instance.totalGold < unlockCost)
        {
            return;
        }
        
        SetUnlocked (true);
        GameManager.Instance.ShowNextResource ();
        
        AchievmentController.Instance.UnlockAchievement (AchievementType.UnlockResource, _config.name);
    }

    public void SetConfig(ResourceConfig config)
    {
        _config = config;

        resourceDescription.text = $"{_config.name} Lv. {_level}\n+{GetOutput().ToString("0")}";
        resourceUnlockCost.text = $"Unlock Cost\n{_config.unlockCost}";
        resourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";
        
        SetUnlocked (_config.unlockCost == 0);
    }
    
    public void SetUnlocked (bool unlocked)
    {
        IsUnlocked = unlocked;
        resourceImage.color = IsUnlocked ? Color.white : Color.grey;
        resourceUnlockCost.gameObject.SetActive (!unlocked);
        resourceUpgradeCost.gameObject.SetActive (unlocked);

    }

    public double GetOutput()
    {
        return _config.output * _level;
    }

    public double GetUpgradeCost()
    {
        return _config.upgradeCost * _level;
    }

    public double GetUnlockCost()
    {
        return _config.unlockCost;
    }

    public void UpgradeLevel()
    {
        var upgradeCost = GetUpgradeCost();
        if (GameManager.Instance.totalGold < upgradeCost)
        {
            return;
        }
        
        GameManager.Instance.AddGold(-upgradeCost);

        _level++;
        
        resourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";
        resourceDescription.text = $"{_config.name} Lv. {_level}\n+{GetOutput().ToString("0")}";
    }
}