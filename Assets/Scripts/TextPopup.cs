using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    [SerializeField] private float lifetime;
    private float maxLifetime;

    [SerializeField] private Vector2 speed;

    [SerializeField] private float disappearSpeed;

    private TextMeshPro textMesh;

    private Color textColor;

    private static int sortingOrder;

    public static TextPopup Create(Vector3 position, string text)
    {
        GameObject textPopup = Instantiate(GameAssets.Instance.textPopupPF, position, Quaternion.identity);

        TextPopup damagePopup = textPopup.GetComponent<TextPopup>();

        damagePopup.Setup(text);

        return damagePopup;
    }

    public void Setup(string text)
    {
        textMesh.SetText(text);

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
    }

    private void Awake()
    {
        textMesh = this.transform.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        Movements();

        lifetime -= Time.deltaTime;

        if (lifetime <= 0)
            Disappear();
    }

    private void Movements()
    {
        this.transform.position += (Vector3)speed * Time.deltaTime;
    }

    private void Disappear()
    {
        textColor = textMesh.color;
        textColor.a -= disappearSpeed * Time.deltaTime;
        textMesh.color = textColor;
        if (textColor.a <= 0)
        {
            sortingOrder--;
            Destroy(this.gameObject);
        }
    }
}
