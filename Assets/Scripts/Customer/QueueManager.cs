using UnityEngine;
using System.Collections.Generic;

public class QueueManager : MonoBehaviour
{
    [SerializeField] private Transform cupReturn;
    [SerializeField] private GameObject customer;
    [SerializeField] private float spawnX;
    [SerializeField] private float spawnY;
    [SerializeField] private float customerSpacing = 0.8f;
    private List<GameObject> customers;
    
    void Awake()
    {
        customers = new List<GameObject>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            AddCustomer();
        }
        else if (Input.GetKeyDown("s"))
        {
            CustomerServed();
        }
    }
    
    public void AddCustomer()
    {
        Vector3 spawnPoint = new Vector3(spawnX, spawnY, 0);
        GameObject newCustomer = Instantiate(customer, spawnPoint,
                                     Quaternion.identity, transform);
        customers.Add(newCustomer);
        Customer newCustomerScript = newCustomer.GetComponent<Customer>();
        newCustomerScript.targetPosition = Vector3.right * customerSpacing
                                           * customers.Count;
        newCustomerScript.cupReturn = cupReturn;
    }
    
    public void CustomerServed()
    {
        Customer customer = customers[0].GetComponent<Customer>();
        customer.hasBeenServed = true;
        customer.Leave();
    }
    
    public void CustomerLeave(GameObject customer)
    {
        customers.Remove(customer);
        ReassignTargetPositions();
    }
    
    private void ReassignTargetPositions()
    {
        for (int i = 0; i < customers.Count; i++)
        {
            Customer customer = customers[i].GetComponent<Customer>();
            customer.targetPosition = Vector3.right * customerSpacing
                                      * (i + 1);
        }
    }
}
