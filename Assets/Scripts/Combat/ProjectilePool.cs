using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
  public static ProjectilePool Instance { get; private set; }

  [SerializeField] private GameObject projectilePrefab;
  [SerializeField] private int initialPoolSize = 15;

  private Queue<Projectile> _pool = new Queue<Projectile>();

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Destroy(gameObject);
      return;
    }

    InitializePool();
  }

  private void InitializePool()
  {
    for (int i = 0; i < initialPoolSize; i++)
    {
      GameObject obj = Instantiate(projectilePrefab, transform);
      obj.SetActive(false);
      Projectile proj = obj.GetComponent<Projectile>();
      proj.SetPool(this);
      _pool.Enqueue(proj);
    }
  }

  public Projectile Get()
  {
    Projectile proj;
    if (_pool.Count > 0)
    {
      proj = _pool.Dequeue();
    }
    else
    {
      // Se a piscina esvaziar, cria um novo dinamicamente para evitar travar o jogo
      GameObject obj = Instantiate(projectilePrefab, transform);
      proj = obj.GetComponent<Projectile>();
      proj.SetPool(this);
    }

    proj.gameObject.SetActive(true);
    return proj;
  }

  public void ReturnToPool(Projectile proj)
  {
    proj.gameObject.SetActive(false);
    _pool.Enqueue(proj);
  }
}