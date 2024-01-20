using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour, IInitialization
{
    [SerializeField] private Transform _pf_Shield;
    [SerializeField] private float _shieldLevelInterval = 0.4f;
    [SerializeField] private GameBoardRuleSO _basicGameRuleSO;

    public List<Card_Shield> ShieldSpriteRenders = new List<Card_Shield>();

    public void IAwake()
    {
    }

    public void IStart()
    {
        InitShields();
    }

    private void InitShields()
    {
        for (int i = 0; i < _basicGameRuleSO.ShieldMaximumNumber; i++)
        {
            Transform currentShield = Instantiate(_pf_Shield, transform);
            ShieldSpriteRenders.Add(currentShield.GetComponent<Card_Shield>());
            ShieldSpriteRenders[i].ShieldEnable(false);
            currentShield.transform.localScale = currentShield.transform.localScale + (Vector3.one * _shieldLevelInterval * i);
        }
    }
}
