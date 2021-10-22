using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // GNA = Gunk DNA
    private int GNA = 0;

    public int GNAtankSize = 100;

    private int GNA_TANK_COUNT = 0;
    private int GNA_TANK_ROWS = 1;

    [SerializeField] private GunkTrackCounter _gunkTrackCounter;
    [SerializeField] private GameObject GNA_TANK;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private Canvas UI;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _gunkTrackCounter.OnVariableChange += addGNA;

        _progressBar.maximum = GNAtankSize;
        _progressBar.minimum = 0;
    }

    public void addGNA(int amount)
    {
        if (GNA_TANK_COUNT == 16) return;
        GNA += amount;
        _progressBar.current = GNA;

        if (_progressBar.current >= _progressBar.maximum)
        {
            _progressBar.minimum = _progressBar.maximum;
            _progressBar.maximum += GNAtankSize;
            GNA_TANK.SetActive(true);
            GNA_TANK = Instantiate(GNA_TANK, UI.transform);
            GNA_TANK_COUNT++;
            GNA_TANK.transform.localPosition += new Vector3(80, 0, 0);
            if (GNA_TANK_COUNT == 8)
            {
                GNA_TANK.transform.localPosition -= new Vector3(80 * 8, -90, 0);
            }
            GNA_TANK.SetActive(false);
            
        }
    }

    public void removeGNA(int amount)
    {
        GNA -= amount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
