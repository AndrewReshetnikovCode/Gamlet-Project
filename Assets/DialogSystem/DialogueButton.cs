using UnityEngine;

public class DialogueButtonController : MonoBehaviour
{
    [SerializeField] private float enlargedScale = 1.3f; // Увеличенный размер
    [SerializeField] private float normalScale = 1f; // Обычный размер
    [SerializeField] public int numberOfChoiseButton;
    [SerializeField] private AudioClip mouseEnterSound;
    [SerializeField] private AudioClip clickSound;
    [HideInInspector] public bool isActive = true;
    private bool _isSoundPlayed = false;

    private bool isEnlarged = false; // Отслеживание текущего состояния размера объекта

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (isActive == true)
        {

            // Кастим луч из позиции мыши и проверяем, попал ли он в этот объект
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject && !isEnlarged)
                {

                    // Увеличиваем размер, если луч попал на этот объект и размер еще не был увеличен
                    transform.localScale = Vector3.one * enlargedScale;
                    gameObject.GetComponent<AudioSource>().PlayOneShot(mouseEnterSound);
                    isEnlarged = true;
                }
                else if (hit.collider.gameObject != gameObject && isEnlarged)
                {
                    // Возвращаем размер, если луч больше не попадает на этот объект
                    ResetScale();
                }
            }
            else if (isEnlarged)
            {
                // Если луч не попал ни в какой объект, возвращаем размер
                ResetScale();
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (DialogueManager.Instance.isDialogueActive == false && isEnlarged)
                {
                    if (_isSoundPlayed == false)
                    {
                        gameObject.GetComponent<AudioSource>().PlayOneShot(clickSound);
                        _isSoundPlayed = true;
                    }


                    DialogueManager.Instance.Choise(numberOfChoiseButton);
                    DialogueManager.Instance.isDialogueActive = true;

                    var c1 = DialogueManager.Instance.choise1.GetComponentInChildren<DialogueButtonController>();
                    var c2 = DialogueManager.Instance.choise2.GetComponentInChildren<DialogueButtonController>();

                    c1.isActive = false;
                    c2.isActive = false;

                    c1.gameObject.GetComponentInChildren<Animator>().SetTrigger("Out");
                    c2.gameObject.GetComponentInChildren<Animator>().SetTrigger("Out");

                    Destroy(DialogueManager.Instance.choise1, 0.667f);
                    Destroy(DialogueManager.Instance.choise2, 0.667f);
                }
            }
        }

    }

    private void ResetScale()
    {
        transform.localScale = Vector3.one * normalScale;
        isEnlarged = false;
    }
}
