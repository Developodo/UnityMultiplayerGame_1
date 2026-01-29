using UnityEngine;
using Unity.Netcode;
using System.Collections;



public class MovementPlayer : NetworkBehaviour
{
    [Header("Movimiento")]
    public float moveForce = 5f;
    public float maxSpeed = 50f; // Velocidad máxima editable en Inspector

    private Rigidbody rb;
    private Renderer render;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)  // Solo el servidor ejecuta este bloque
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-5f, 5f),
                2f,
                Random.Range(-5f, 5f)
            );
            /* Dado que transform object tiene activado transform sync, la nueva posición se replicará automáticamente a los clientes */
            transform.position = randomPos;
            Color c = new Color(Random.value, Random.value, Random.value);
            UpdateColorClientRpc(c);
        }
  
    }

    /*IEnumerator Start()
    {
        yield return new WaitForSeconds(10f);

        if (IsServer)
        {
            transform.position += Vector3.forward * 5;
        }
    }*/

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        render = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Solo el propietario del objeto puede enviar solicitudes de movimiento
        if(!IsOwner) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0, v);
        
        // Enviar solicitud de movimiento al servidor si hay entrada
        if (direction.sqrMagnitude > 0.01f)
            MoveRequestRpc(direction);

    }

    [Rpc(SendTo.Server)]
    private void MoveRequestRpc(Vector3 direction)
    {

        // Aplicar fuerza en servidor
        rb.AddForce(direction * moveForce, ForceMode.Force);

        // Limitar velocidad
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        
            // Actualizar posición en clientes
            UpdatePositionClientRpc(transform.position);

    }
    [Rpc(SendTo.ClientsAndHost)]
    private void UpdatePositionClientRpc(Vector3 pos)
    {
        // Evitar que el propietario sobrescriba su propia posición
        if (IsServer) return;
        // Actualizar posición en clientes
        transform.position = pos;
    }

        //Solución Tarea UT2
    [Rpc(SendTo.Server)]
    public void ChangeColorRandomRpc()
    { 
        Color c = new Color(Random.value, Random.value, Random.value);
        UpdateColorClientRpc(c);
        
        
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void UpdateColorClientRpc(Color c)
    {
        if (render != null)
            render.material.color=c;
    }

    
    
}
