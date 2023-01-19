using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    public class Button_Custom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        private ObjectPooler objectPooler;
        private EventSystem eventSystem;

        [SerializeField] protected Image image;

        //Tween values
        [SerializeField, FoldoutGroup("Scale")] protected float startSizeX;
        [SerializeField, FoldoutGroup("Scale")] protected float scaleMultiplier = 1.1f;
        [SerializeField, FoldoutGroup("Scale")] protected float TweenTime = 0.1f;
        [SerializeField, FoldoutGroup("Scale")] protected LeanTweenType tweenTypeIn;
        [SerializeField, FoldoutGroup("Scale")] protected LeanTweenType tweenTypeOut;

        [SerializeField, FoldoutGroup("Color")] protected Color baseColor = new Color(1,1,1,1);
        [SerializeField, FoldoutGroup("Color")] protected Color hoverColor;
        [SerializeField, FoldoutGroup("Color")] protected Color clickColor;

        [SerializeField, FoldoutGroup("Rotate")] protected float startRotationZ;
        [SerializeField, FoldoutGroup("Rotate")] protected LeanTweenType rotateTypeIn;
        [SerializeField, FoldoutGroup("Rotate")] protected LeanTweenType rotateTypeOut;
        [SerializeField, FoldoutGroup("Rotate")] protected float rotateTime = 0.1f;

        [SerializeField, FoldoutGroup("PixelsPerUnit")] protected float startPPU;
        [SerializeField, FoldoutGroup("PixelsPerUnit")] protected LeanTweenType PPUTypeIn;
        [SerializeField, FoldoutGroup("PixelsPerUnit")] protected LeanTweenType PPUTypeOut;
        [SerializeField, FoldoutGroup("PixelsPerUnit")] protected float PPUTime = 0.1f;

        //SFX
        protected AudioSource audioSource;
        [SerializeField] protected AudioClip hoverSFX;
        [SerializeField] protected AudioClip clickSFX;

        [SerializeField] protected UnityEvent OnPointerEnterActions;
        [SerializeField] protected UnityEvent OnPointerExitActions;
        [SerializeField] protected UnityEvent OnPointerDownActions;
        [SerializeField] protected UnityEvent OnPointerUpActions;
        [SerializeField] protected UnityEvent OnPointerClickActions;

        protected virtual void Awake()
        {
            startSizeX = transform.localScale.x;
            startRotationZ = transform.localRotation.z;
            if(image != null){startPPU = image.pixelsPerUnitMultiplier;}
            audioSource = GetComponent<AudioSource>();
            eventSystem = EventSystem.current;
        }

        protected virtual void Start()
        {
            objectPooler = ObjectPooler.Instance;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            UIManager.currentHoveringButton = gameObject;
            OnPointerEnterActions.Invoke();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            UIManager.currentHoveringButton = null;
            OnPointerExitActions.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownActions.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(UIManager.currentHoveringButton == gameObject)
            {
                OnPointerUpActions.Invoke();
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            OnPointerClickActions.Invoke();
        }

        public void EnableDisableGameobject(GameObject go)
        {
            go.SetActive(!go.activeSelf);
        }

        #region FX

        public virtual void PlayHoverSFX()
        {
            if(hoverSFX == null)
            {
                return;
            }
            audioSource.clip = hoverSFX;
            audioSource.Play();
        }

        public virtual void PlayClickSFX()
        {
            if (clickSFX == null)
            {
                return;
            }
            audioSource.clip = clickSFX;
            audioSource.Play();
        }

        public virtual void ScaleOriginal()
        {
            LeanTween.scaleX(image.gameObject, startSizeX, TweenTime).setIgnoreTimeScale(true).setEase(tweenTypeOut);
        }

        public virtual void ScaleUp()
        {
            LeanTween.scaleX(image.gameObject, startSizeX * scaleMultiplier, TweenTime).setIgnoreTimeScale(true).setEase(tweenTypeIn);
        }

        public virtual void PPUOriginal()
        {
            LeanTween.value(image.gameObject,image.pixelsPerUnitMultiplier, startPPU, PPUTime).setOnUpdate((float val) => { image.pixelsPerUnitMultiplier = val; }).setIgnoreTimeScale(true).setEase(PPUTypeOut);
        }

        public virtual void PPUUp(float to)
        {
            LeanTween.value(image.gameObject, image.pixelsPerUnitMultiplier, to, PPUTime).setOnUpdate((float val) => { image.pixelsPerUnitMultiplier = val; }).setIgnoreTimeScale(true).setEase(PPUTypeIn);
        }

        public virtual void ChangeColorBase()
        {
            image.color = baseColor;
        }

        public virtual void ChangeColorHover()
        {
            image.color = hoverColor;
        }

        public virtual void ChangeColorClick()
        {
            image.color = clickColor;
        }

        public virtual void RotateIn(float degrees)
        {
            LeanTween.rotateZ(image.gameObject, startRotationZ + degrees, rotateTime).setIgnoreTimeScale(true);
        }

        public virtual void RotateBack()
        {
            LeanTween.rotateZ(image.gameObject, startRotationZ, rotateTime).setIgnoreTimeScale(true);
        }

        #endregion
    }
}
