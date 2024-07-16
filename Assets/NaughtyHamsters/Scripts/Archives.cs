using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archives : MonoBehaviour
{
    /* ------------------------------------------- */
    /* ---------------- Player.cs ---------------- */
    /* ------------------------------------------- */


    ///// <summary>
    ///// UI Slider visualizing calorie value.
    ///// </summary>
    //public Slider calorieSlider;

    ///// <summary>
    ///// Current turret rotation
    ///// </summary>
    //[HideInInspector] public short turretRotation;

    //void Start()
    //{
    //    //call hooks manually to update
    //    OnCalorieChange(GetView().GetCalorie());
    //}

    //public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player player, ExitGames.Client.Photon.Hashtable playerAndUpdatedProps)
    //{
    //    OnCalorieChange(player.GetCalorie());
    //}

    ////hook for updating calorie locally (the actual value updates via player properties)
    //protected void OnCalorieChange(int value)
    //{
    //    // calorieSlider.value = (float)value / maxCalorie;
    //}

    ////hook for updating turret rotation locally
    //void OnTurretRotation()
    //{
    //    //we don't need to check for local ownership when setting the turretRotation,
    //    //because OnPhotonSerializeView PhotonStream.isWriting == true only applies to the owner
    //    turret.rotation = Quaternion.Euler(0, turretRotation, 0);
    //}

    //        //continously check for input on desktop platforms
    //#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
    //        void FixedUpdate()
    //		{
    //			//skip further calls for remote clients    
    //            if (!photonView.IsMine)
    //            {
    //                //keep turret rotation updated for all clients
    //                OnTurretRotation();
    //                return;
    //            }

    //            //movement variables
    //            //Vector2 moveDir;
    //            Vector2 turnDir;

    //            ////reset moving input when no arrow keys are pressed down
    //            //if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
    //            //{
    //            //    moveDir.x = 0;
    //            //    moveDir.y = 0;
    //            //}
    //            //else
    //            //{
    //            //    //read out moving directions and calculate force
    //            //    moveDir.x = Input.GetAxis("Horizontal");
    //            //    moveDir.y = Input.GetAxis("Vertical");
    //            //    Move(moveDir);
    //            //}

    //            //cast a ray on a plane at the mouse position for detecting where to shoot 
    //            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //            Plane plane = new Plane(Vector3.up, Vector3.up);
    //            //float distance = 0f;
    //            Vector3 hitPos = Vector3.zero;
    //            //the hit position determines the mouse position in the scene
    //            //if (plane.Raycast(ray, out distance))
    //            //{
    //            //    hitPos = ray.GetPoint(distance) - transform.position;
    //            //}

    //            //we've converted the mouse position to a direction
    //            turnDir = new Vector2(hitPos.x, hitPos.z);

    //            //rotate turret to look at the mouse direction
    //            RotateTurret(new Vector2(hitPos.x, hitPos.z));

    //            //shoot bullet on left mouse click
    //            //if (Input.GetButton("Fire1"))
    //            //    Shoot();

    //            ////replicate input to mobile controls for illustration purposes
    //            //#if UNITY_EDITOR
    //            //	GameManager.GetInstance().ui.controls[0].position = moveDir;
    //            //	GameManager.GetInstance().ui.controls[1].position = turnDir;
    //            //#endif
    //        }
    //        #endif

    //this method gets called multiple times per second, at least 10 times or more
    //void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        //here we send the turret rotation angle to other clients
    //        //stream.SendNext(turretRotation);
    //    }
    //    else
    //    {
    //        //this.turretRotation = (short)stream.ReceiveNext();
    //        //OnTurretRotation();
    //    }
    //}

    /* ------------------------------------------- */
    /* ---------------- UIGame.cs ---------------- */
    /* ------------------------------------------- */

    ///// <summary>
    ///// Set game end text and display winning team in its team color.
    ///// </summary>
    //public void SetGameOverText(Team team)
    //{

    //    //show winning team and colorize it by converting the team color to an HTML RGB hex value for UI markup
    //    gameOverText.text = "TEAM <color=#" + ColorUtility.ToHtmlStringRGB(team.material.color) + ">" + team.name + "</color> WINS!";
    //}

    /* ------------------------------------------- */
    /* ---------------- UIMain.cs ---------------- */
    /* ------------------------------------------- */

    //how many times the shop has been opened
    //private int shopOpened = 0;

    //how many times the settings have been opened
    //private int settingsOpened = 0;

    ///// <summary>
    ///// Increase counter when opening settings.
    ///// Used for Unity Analytics purposes.
    ///// </summary>
    //public void OpenSettings()
    //{
    //    //settingsOpened++;
    //}

    //public void OpenStoryObject()
    //{
    //    //storyObjectOpened++;
    //}

    //public void OpenComponents()
    //{
    //    //componentsOpened++;
    //}
    //public void OpenSetupsPhases()
    //{
    //    //setupsPhasesOpened++;
    //}

    ///// <summary>
    ///// Allow additional input of server address only in network mode LAN.
    ///// Otherwise, the input field will be hidden in the settings (Photon only).
    ///// </summary>
    //public void OnNetworkChanged(int value)
    //{
    //    serverField.gameObject.SetActive((NetworkMode)value == NetworkMode.LAN ? true : false);
    //}

    /* OTHER ADD-ON FUNCTIONS */

    //    /// <summary>
    //    /// Increase counter when opening the shop.
    //    /// Used for Unity Analytics purposes.
    //    /// </summary>
    //    public void OpenShop()
    //    {
    //        //shopOpened++;
    //    }

    //    /// <summary>
    //    /// Opens a browser window to the App Store entry for this app.
    //    /// </summary>
    //    public void RateApp()
    //    {
    //        //UnityAnalyticsManager.RateStart();

    //        //default app url on non-mobile platforms
    //        //replace with your website, for example
    //        string url = "";

    //#if UNITY_ANDROID
    //				url = "http://play.google.com/store/apps/details?id=" + Application.identifier;
    //#elif UNITY_IPHONE
    //				url = "https://itunes.apple.com/app/idXXXXXXXXX";
    //#endif

    //        if (string.IsNullOrEmpty(url) || url.EndsWith("XXXXXX"))
    //        {
    //            Debug.LogWarning("UIMain: You didn't replace your app links!");
    //            return;
    //        }

    //        Application.OpenURL(url);
    //    }

    /* ------------------------------------------- */
    /* ---------------- ParticleColor.cs ---------------- */
    /* ------------------------------------------- */

    //public class ParticleColor : MonoBehaviour
    //{
    //    /// <summary>
    //    /// Array for particle systems that should be colored.
    //    /// </summary>
    //    public ParticleSystem[] particles;

    //    /// <summary>
    //    /// Iterates over all particles and assigns the color passed in,
    //    /// but ignoring the alpha value of the new color.
    //    /// </summary>
    //    public void SetColor(Color color)
    //    {
    //        for (int i = 0; i < particles.Length; i++)
    //        {
    //            var main = particles[i].main;
    //            color.a = main.startColor.color.a;
    //            main.startColor = color;
    //        }
    //    }
    //}

    /* ------------------------------------------- */
    /* ---------------- PoolParticle.cs ---------------- */
    /* ------------------------------------------- */

    //namespace NaughtyHamster
    //{
    ///// <summary>
    ///// When attached to an gameobject containing ParticleSystems,
    ///// handles automatic despawn after the maximum particle duration.
    ///// </summary>
    //public class PoolParticle : MonoBehaviour
    //{
    //    /// <summary>
    //    /// Delay before despawning this gameobject. Calculated based on the ParticleSystem duration,
    //    /// but can be overwritten by setting it to something greater than zero.
    //    /// </summary>
    //    public float delay = 0f;

    //    //references to all ParticleSystem components
    //    private ParticleSystem[] pSystems;


    //    //initialize variables
    //    void Awake()
    //    {
    //        pSystems = GetComponentsInChildren<ParticleSystem>();

    //        //don't continue if delay has been overwritten
    //        //otherwise find the maximum particle duration
    //        if (delay > 0) return;
    //        for (int i = 0; i < pSystems.Length; i++)
    //        {
    //            var main = pSystems[i].main;
    //            if (main.duration > delay)
    //                delay = main.duration;
    //        }
    //    }
    //}


    //    //play particles
    //    void OnSpawn()
    //    {
    //        //loop over ParticleSystem references and play them
    //        //Unity does not seem to calculate a new iteration of particles when
    //        //particles get activated, so here we add a randomized seed to it too
    //        for (int i = 0; i < pSystems.Length; i++)
    //        {
    //            pSystems[i].Stop();
    //            pSystems[i].randomSeed = (uint)Random.Range(0f, uint.MaxValue);
    //            pSystems[i].Play();
    //        }

    //        //set automatic despawn after play duration
    //        PoolManager.Despawn(gameObject, delay);
    //    }

    /* ------------------------------------------- */
    /* ---------------- PoolManager.cs ---------------- */
    /* ------------------------------------------- */

    //namespace NaughtyHamster
    //{
    //    /// <summary>
    //    /// This class provides networked object pooling functionality and stores all Pool references.
    //    /// Spawning and despawning is handled by calling the corresponding methods, but there are also
    //    /// methods for creating Pools at runtime or destroying all gameobjects in a Pool completely.
    //    /// </summary>
    //    public class PoolManager : MonoBehaviour 
    //    {
    //        //mapping of prefab to Pool container managing all of its instances
    //        private static Dictionary<GameObject, Pool> Pools = new Dictionary<GameObject, Pool>();


    //        /// <summary>
    //        /// Called by each Pool on its own, this adds it to the dictionary.
    //        /// </summary>
    //        public static void Add(Pool pool) 
    //        {
    //            //check if the Pool does not contain a prefab
    //            if (pool.prefab == null)
    //            {
    //                Debug.LogError("Prefab of pool: " + pool.gameObject.name + " is empty! Can't add pool to Pools Dictionary.");
    //                return;
    //            }

    //            //check if the Pool has been added already
    //            if(Pools.ContainsKey(pool.prefab))    
    //            {
    //                Debug.LogError("Pool with prefab " + pool.prefab.name + " has already been added to Pools Dictionary.");
    //                return;
    //            }

    //            //add it to dictionary
    //            Pools.Add(pool.prefab, pool);
    //        }


    //        /// <summary>
    //        /// Creates a new Pool at runtime. This is being called for prefabs which have not been linked
    //        /// to a Pool in the scene in the editor, but are called via Spawn() nonetheless.
    //        /// </summary>
    //        public static void CreatePool(GameObject prefab, int preLoad, bool limit, int maxCount)
    //        {
    //            //debug error if pool was already added before 
    //            if (Pools.ContainsKey(prefab))
    //            {
    //                Debug.LogError("Pool Manager already contains Pool for prefab: " + prefab.name);
    //                return;
    //            }

    //            //create new gameobject which will hold the new Pool component
    //            GameObject newPoolGO = new GameObject("Pool " + prefab.name);
    //            //add Pool component to the new gameobject in the scene
    //            Pool newPool = newPoolGO.AddComponent<Pool>();
    //            //assign default parameters
    //            newPool.prefab = prefab;
    //            newPool.preLoad = preLoad;
    //            newPool.limit = limit;
    //            newPool.maxCount = maxCount;
    //            //let it initialize itself after assigning variables
    //            newPool.Awake();
    //        }


    //        /// <summary>
    //        /// Activates a pre-instantiated instance for the prefab passed in, at the desired position.
    //        /// </summary>
    //        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    //        {
    //            //debug a Log entry in case the prefab was not found in a Pool
    //            //this is not critical as then we create a new Pool for it at runtime
    //            if(!Pools.ContainsKey(prefab))
    //            {
    //                Debug.Log("Prefab not found in existing pool: " + prefab.name + " New Pool has been created.");
    //                CreatePool(prefab, 0, false, 0);
    //            }

    //            //spawn instance in the corresponding Pool
    //            return Pools[prefab].Spawn(position, rotation);
    //        }


    //        /// <summary>
    //        /// Disables a previously spawned instance for later use.
    //        /// Optionally takes a time value to delay the despawn routine.
    //        /// </summary>
    //        public static void Despawn(GameObject instance, float time = 0f)
    //        {
    //            if(time > 0) GetPool(instance).Despawn(instance, time);
    //            else GetPool(instance).Despawn(instance);
    //        }


    //        /// <summary>
    //        /// Convenience method for quick lookup of an pooled object.
    //        /// Returns the Pool component where the instance has been found in.
    //        /// </summary>
    //        public static Pool GetPool(GameObject instance)
    //        {
    //            //go over Pools and find the instance
    //            foreach (GameObject prefab in Pools.Keys)
    //            {
    //                if(Pools[prefab].active.Contains(instance))
    //                    return Pools[prefab];
    //            }

    //            //the instance could not be found in a Pool
    //            Debug.LogError("PoolManager couldn't find Pool for instance: " + instance.name);
    //            return null;
    //        }


    //        /// <summary>
    //        /// Despawns all instances of a Pool, making them available for later use.
    //        /// </summary>
    //        public static void DeactivatePool(GameObject prefab)
    //        {
    //            //debug error if Pool wasn't already added before
    //            if (!Pools.ContainsKey(prefab))
    //            {
    //                Debug.LogError("PoolManager couldn't find Pool for prefab to deactivate: " + prefab.name);
    //                return;
    //            }

    //            //cache active count
    //            int count = Pools[prefab].active.Count;
    //            //loop through each active instance
    //            for (int i = count - 1; i > 0; i--)
    //            {
    //                Pools[prefab].Despawn(Pools[prefab].active[i]);
    //            }
    //        }


    //        /// <summary>
    //        /// Destroys all despawned instances of all Pools to free up memory.
    //        /// The parameter 'limitToPreLoad' decides if only instances above the preload
    //        /// value should be destroyed, to keep a minimum amount of disabled instances
    //        /// </summary>
    //        public static void DestroyAllInactive(bool limitToPreLoad)
    //        {
    //            foreach (GameObject prefab in Pools.Keys)
    //                Pools[prefab].DestroyUnused(limitToPreLoad);
    //        }


    //        /// <summary>
    //        /// Destroys the Pool for a specific prefab.
    //        /// Active or inactive instances are not available anymore after calling this.
    //        /// </summary>
    //        public static void DestroyPool(GameObject prefab)
    //        {
    //            //debug error if Pool wasn't already added before
    //            if (!Pools.ContainsKey(prefab))
    //            {
    //                Debug.LogError("PoolManager couldn't find Pool for prefab to destroy: " + prefab.name);
    //                return;
    //            }

    //            //destroy pool gameobject including all children. Our game logic doesn't reparent instances,
    //            //but if you do, you should loop over the active and deactive instances manually to destroy them
    //            Destroy(Pools[prefab].gameObject);
    //            //remove key-value pair from dictionary
    //            Pools.Remove(prefab);
    //        }


    //        /// <summary>
    //        /// Destroys all Pools stored in the manager dictionary.
    //        /// Active or inactive instances are not available anymore after calling this.
    //        /// </summary>
    //        public static void DestroyAllPools()
    //        {
    //            //loop over dictionary and destroy every pool gameobject
    //            //see DestroyPool method for further comments
    //            foreach (GameObject prefab in Pools.Keys)
    //                DestroyPool(Pools[prefab].gameObject);
    //        }


    //        //static variables always keep their values over scene changes
    //        //so we need to reset them when the game ended or switching scenes
    //        void OnDestroy()
    //        {
    //            Pools.Clear();
    //        }
    //    }
    //}

    /* ------------------------------------------- */
    /* ---------------- Pool.cs ---------------- */
    /* ------------------------------------------- */

    //namespace NaughtyHamster
    //{
    //    /// <summary>
    //    /// Child class interacting and managed by the PoolManager.
    //    /// Handles all internal spawning/despawning of active/inactive instances.
    //    /// </summary>
    //    public class Pool : MonoBehaviour
    //    {
    //        /// <summary>
    //        /// Prefab to instantiate for pooling.
    //        /// </summary>
    //        public GameObject prefab;

    //        /// <summary>
    //        /// Amount of instances to create at game start.
    //        /// </summary>
    //        public int preLoad = 0;

    //        /// <summary>
    //        /// Whether the creation of new instances should be limited at runtime.
    //        /// </summary>
    //        public bool limit = false;

    //        /// <summary>
    //        /// Maximum amount of instances created, if limit is enabled.
    //        /// </summary>
    //        public int maxCount;

    //        /// <summary>
    //        /// List of active prefab instances for this pool.
    //        /// </summary>  
    //        [HideInInspector]
    //        public List<GameObject> active = new List<GameObject>();

    //        /// <summary>
    //        /// List of inactive prefab instances for this pool.
    //        /// </summary>
    //        [HideInInspector]
    //        public List<GameObject> inactive = new List<GameObject>();

    //        public void Awake()
    //        {
    //            if (prefab == null) return;

    //            //add this pool to the PoolManager dictionary
    //            PoolManager.Add(this);
    //            PreLoad();
    //        }

    //        /// <summary>
    //        /// Loads specified amount of objects before playtime.
    //        /// </summary>
    //        public void PreLoad()
    //        {
    //            if (prefab == null)
    //            {
    //                Debug.LogWarning("Prefab in pool empty! No Preload happening. Please check references.");
    //                return;
    //            }

    //            //instantiate defined preload amount but don't exceed the maximum amount of objects
    //            for (int i = totalCount; i < preLoad; i++)
    //            {
    //                //instantiate new instance of the prefab
    //                GameObject obj = (GameObject)Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
    //                //parent the new instance to this transform
    //                obj.transform.SetParent(transform);

    //                //rename it to an unique heading for easier editor overview
    //                Rename(obj.transform);
    //                //deactivate object including child objects
    //                obj.SetActive(false);
    //                //add object to the list of inactive instances
    //                inactive.Add(obj);
    //            }
    //        }


    //        /// <summary>
    //        /// Activates (or instantiates) a new instance for this pool.
    //        /// </summary>
    //        public GameObject Spawn(Vector3 position, Quaternion rotation)
    //        {
    //            GameObject obj;
    //            Transform trans;

    //            //there are inactive objects available for activation
    //            if (inactive.Count > 0)
    //            {
    //                //get first inactive object in the list
    //                obj = inactive[0];
    //                //we want to activate it, remove it from the inactive list
    //                inactive.RemoveAt(0);

    //                //get instance transform
    //                trans = obj.transform;
    //            }
    //            else
    //            {
    //                //we don't have any inactive objects available,
    //                //we have to instantiate a new one
    //                //check if the limited count allows new instantiations
    //                //if not, return nothing
    //                if (limit && active.Count >= maxCount)
    //                    return null;

    //                //instantiation possible, instantiate new instance of the prefab
    //                obj = (GameObject)Object.Instantiate(prefab);
    //                //get instance transform
    //                trans = obj.transform;
    //                //rename it to an unique heading for easier editor overview
    //                Rename(trans);
    //            }

    //            //set position and rotation passed in
    //            trans.position = position;
    //            trans.rotation = rotation;
    //            //in case it wasn't parented to this transform, reparent it now
    //            if (trans.parent != transform)
    //                trans.parent = transform;

    //            //add object to the list of active instances
    //            active.Add(obj);
    //            //activate object including child objects
    //            obj.SetActive(true);
    //            //call the method OnSpawn() on every component and children of this object
    //            obj.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

    //            //submit instance
    //            return obj;
    //        }


    //        /// <summary>
    //        /// Deactivates an instance of this pool for later use.
    //        /// </summary>
    //        public void Despawn(GameObject instance)
    //        {
    //            //search in active instances for this instance
    //            if (!active.Contains(instance))
    //            {
    //                Debug.LogWarning("Can't despawn - Instance not found: " + instance.name + " in Pool " + this.name);
    //                return;
    //            }

    //            //in case it was unparented during runtime, reparent it now
    //            if (instance.transform.parent != transform)
    //                instance.transform.parent = transform;

    //            //we want to deactivate it, remove it from the active list
    //            active.Remove(instance);
    //            //add object to the list of inactive instances instead
    //            inactive.Add(instance);
    //            //call the method OnDespawn() on every component and children of this object
    //            instance.BroadcastMessage("OnDespawn", SendMessageOptions.DontRequireReceiver);
    //            //deactivate object including child objects
    //            instance.SetActive(false);
    //        }


    //        /// <summary>
    //        /// Timed deactivation of an instance of this pool for later use.
    //        /// </summary>
    //        public void Despawn(GameObject instance, float time)
    //        {
    //            //create new class PoolTimeObject to keep track of the instance
    //            PoolTimeObject timeObject = new PoolTimeObject();
    //            //assign time and instance variable of this class
    //            timeObject.instance = instance;
    //            timeObject.time = time;

    //            //start timed deactivation using the created properties
    //            StartCoroutine(DespawnInTime(timeObject));
    //        }


    //        //coroutine which waits for 'time' seconds before deactivating the instance
    //        IEnumerator DespawnInTime(PoolTimeObject timeObject)
    //        {
    //            //cache instance to deactivate
    //            GameObject instance = timeObject.instance;

    //            //wait for defined seconds
    //            float timer = Time.time + timeObject.time;
    //            while (instance.activeInHierarchy && Time.time < timer)
    //                yield return null;

    //            //the instance got deactivated in between already
    //            if (!instance.activeInHierarchy) yield break;
    //            //despawn it now
    //            Despawn(instance);
    //        }


    //        /// <summary>
    //        /// Destroys all inactive instances of this pool (garbage collector heavy). The
    //        /// parameter determines if only instances above the preLoad value should be destroyed.
    //        /// </summary>
    //        public void DestroyUnused(bool limitToPreLoad)
    //        {
    //            //only destroy instances above the limit amount
    //            if (limitToPreLoad)
    //            {
    //                //start from the last inactive instance and count down
    //                //until the index reached the limit amount
    //                for (int i = inactive.Count - 1; i >= preLoad; i--)
    //                {
    //                    //destroy the object at 'i'
    //                    Object.Destroy(inactive[i]);
    //                }
    //                //remove the range of destroyed objects (now null references) from the list
    //                if (inactive.Count > preLoad)
    //                    inactive.RemoveRange(preLoad, inactive.Count - preLoad);
    //            }
    //            else
    //            {
    //                //limitToPreLoad is false, destroy all inactive instances
    //                for (int i = 0; i < inactive.Count; i++)
    //                {
    //                    Object.Destroy(inactive[i]);
    //                }
    //                //reset the list
    //                inactive.Clear();
    //            }
    //        }


    //        /// <summary>
    //        /// Destroys a specific amount of inactive instances (garbage collector heavy).
    //        /// </summary>
    //        public void DestroyCount(int count)
    //        {
    //            //the amount which was passed in exceeds the amount of inactive instances
    //            if (count > inactive.Count)
    //            {
    //                Debug.LogWarning("Destroy Count value: " + count + " is greater than inactive Count: " +
    //                                inactive.Count + ". Destroying all available inactive objects of type: " +
    //                                prefab.name + ". Use DestroyUnused(false) instead to achieve the same.");
    //                DestroyUnused(false);
    //                return;
    //            }

    //            //starting from the end, count down the index and destroy each inactive instance
    //            //until we destroyed the amount passed in
    //            for (int i = inactive.Count - 1; i >= inactive.Count - count; i--)
    //            {
    //                Object.Destroy(inactive[i]);
    //            }
    //            //remove the range of destroyed objects (now null references) from the list
    //            inactive.RemoveRange(inactive.Count - count, count);
    //        }


    //        //create an unique name for each instance at instantiation
    //        //to differ them from each other in the editor
    //        private void Rename(Transform instance)
    //        {
    //            //count total instances and assign the next free number
    //            //convert it in the range of hundreds:
    //            //there shouldn't be thousands of instances at any time
    //            //e.g. TestEnemy(Clone)001
    //            instance.name += (totalCount + 1).ToString("#000");
    //        }


    //        //count all instances of this pool option
    //        private int totalCount
    //        {
    //            get
    //            {
    //                //initialize count value
    //                int count = 0;
    //                //add active and inactive count
    //                count += active.Count;
    //                count += inactive.Count;
    //                //return final count
    //                return count;
    //            }
    //        }


    //        //when this pool gets destroyed,
    //        //clear instances lists
    //        void OnDestroy()
    //        {
    //            active.Clear();
    //            inactive.Clear();
    //        }
    //    }


    //    /// <summary>
    //    /// Stores properties used on timed deactivation of instances.
    //    /// </summary>
    //    [System.Serializable]
    //    public class PoolTimeObject
    //    {
    //        /// <summary>
    //        /// Instance to deactivate.
    //        /// </summary>
    //        public GameObject instance;

    //        /// <summary>
    //        /// Delay until deactivation.
    //        /// </summary>
    //        public float time;
    //    }
    //}
}
