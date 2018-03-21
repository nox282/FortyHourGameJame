using System.Collections.Generic;
using UnityEngine;

public class PatientFactory : MonoBehaviour
{
    public GameObject patientPrefab;
    public DataBank bank;

    private const int MAX_SYMPTOMS_COUNT = 3;


    public GameObject CreatePatient(Vector3 position)
    {
        GameObject patient = Instantiate(patientPrefab, position, Quaternion.identity) as GameObject;

        int symptomsCount = Random.Range(0, MAX_SYMPTOMS_COUNT);
        List<int> assignedSymptoms = new List<int>();

        for (int i = 0; i < symptomsCount; i++)
        {
            int symptomIndex;
            do
            {
                symptomIndex = Random.Range(0, bank.symptoms.Count - 1);
            }
            while (assignedSymptoms.Contains(symptomIndex));
             
            patient.GetComponent<Patient>().AddSymptom(bank.symptoms[symptomIndex].GetComponent<Symptom>());
        }

        return patient;
    }
}
