using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    // Una variable estática para mantener una referencia a la única instancia de esta clase.
    public static PersistentObject Instance { get; private set; }

    void Awake()
    {
        // Esto se ejecuta antes del método Start().

        // Comprobamos si ya existe una instancia.
        if (Instance == null)
        {
            // Si no existe, esta se convierte en la instancia.
            Instance = this;
            // Y le decimos a Unity que no la destruya al cambiar de escena.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si ya existe una instancia (porque volvimos a la escena principal, por ejemplo),
            // destruimos este nuevo objeto duplicado para mantener solo el original.
            Destroy(gameObject);
        }
    }
}