using UnityEngine;
using System.Collections;

public class Actions : MonoBehaviour {
    

    public string m_sName       = "";
    public string m_sTag        = "";

    public float m_fCoolDown   = 0f;
    private float m_fAuxCd      = 0f;

    void Start() {
        OnStart();
    }

    public virtual void OnStart()
    {
        m_fAuxCd = m_fCoolDown; 
    }

    public void execute() {
        if (canExecute()) {
            m_fAuxCd = 0;
            Action();
        }
        else {
            Debug.LogWarning("Hey, this spell is not ready yet.");
        }
    }

    public virtual void Action() {
        Debug.LogError("Hey, you have an action, that don't run any execute()!!! Please, lookUp to " + m_sName);
    }

    void Update() {
        OnUpdate();
    }

    public virtual void OnUpdate() {
        m_fAuxCd += Time.deltaTime; 
    }

    public float coolDown {
        set{
            m_fCoolDown = value;
        }
        get{
            return m_fCoolDown;
        }
    }

    private bool canExecute(){
        return m_fAuxCd >= m_fCoolDown;
    }

    public string getName(){
        return m_sName;
    }

}
