using System.Linq;
using Symnity.Infrastructure;
using Symnity.Model.Accounts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LessonBasic1 : MonoBehaviour
{
    [SerializeField] private Button lesson1Button;
    [SerializeField] private TMP_InputField addressInputField;
    [SerializeField] private TMP_InputField mosaicIdInputField;
    [SerializeField] private TextMeshProUGUI mosaicAmount;

    private AccountRepository accountRepository;

    private void Start()
    {
        
        accountRepository = new AccountRepository("NODE_URL");
        lesson1Button.onClick.AddListener(GetMosaic);
    }
    
    private async void GetMosaic()
    {
        var address = Address.CreateFromRawAddress(addressInputField.text);
        var mosaicId = mosaicIdInputField.text;
        var accountInfo = await accountRepository.GetAccountInfo(address);
        var mosaic = accountInfo.mosaics.Select(mosaic => mosaic.Id.GetIdAsHex() == mosaicId ? mosaic : null);
        mosaicAmount.text = mosaic.ToList()[0].Amount.ToString();
    }
}