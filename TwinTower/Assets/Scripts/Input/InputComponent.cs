using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace TwinTower
{
    public abstract class InputComponent<T> : Manager<T> where T : InputComponent<T>
    {
        public enum InputType
        {
            MouseAndKeyboard,
            Controller,
        }
        [SerializeField]
        public class InputButton
        {
            public KeyCode key;
            public bool Down { get; protected set; }
            public bool Held { get; protected set; }
            public bool Up { get; protected set; }
            public bool Enabled
            {
                get { return _Enabled; }
            }

            [SerializeField]
            protected bool _Enabled = true;
            protected bool _GettingInput = true;

            // This is used to change the state of a button (Down, Up) only if at least a FixedUpdate happened between the previous Frame
            // and this one. Since movement are made in FixedUpdate, without that an input could be missed it get press/release between fixedupdate
            private bool _AfterFixedUpdateDown;
            private bool _AfterFixedUpdateHeld;
            private bool _AfterFixedUpdateUp;


            public InputButton(KeyCode key)
            {
                this.key = key;
            }

            public void Get(bool fixedUpdateHappened, InputType inputType)
            {
                if (!_Enabled)
                {
                    Down = false;
                    Held = false;
                    Up = false;
                    return;
                }

                if (!_GettingInput)
                    return;

                if (inputType == InputType.MouseAndKeyboard)
                {
                    if (fixedUpdateHappened)
                    {
                        Down = Input.GetKeyDown(key);
                        Held = Input.GetKey(key);
                        Up = Input.GetKeyUp(key);

                        _AfterFixedUpdateDown = Down;
                        _AfterFixedUpdateHeld = Held;
                        _AfterFixedUpdateUp = Up;
                    }
                    else
                    {
                        Down = Input.GetKeyDown(key) || _AfterFixedUpdateDown;
                        Held = Input.GetKey(key) || _AfterFixedUpdateHeld;
                        Up = Input.GetKeyUp(key) || _AfterFixedUpdateUp;

                        _AfterFixedUpdateDown |= Down;
                        _AfterFixedUpdateHeld |= Held;
                        _AfterFixedUpdateUp |= Up;
                    }
                }
            }

            public void Enable()
            {
                _Enabled = true;
            }

            public void Disable()
            {
                _Enabled = false;
            }

            public void GainControl()
            {
                _GettingInput = true;
            }

            public IEnumerator ReleaseControl(bool resetValues)
            {
                _GettingInput = false;

                if (!resetValues)
                    yield break;

                if (Down)
                    Up = true;
                Down = false;
                Held = false;

                _AfterFixedUpdateDown = false;
                _AfterFixedUpdateHeld = false;
                _AfterFixedUpdateUp = false;

                yield return null;

                Up = false;
            }
        }
        
        
        public class InputAxis
        {
            public KeyCode positive;
            public KeyCode negative;
            public float Value { get; protected set; }
            public bool ReceivingInput { get; protected set; }
            public bool Enabled
            {
                get { return m_Enabled; }
            }

            protected bool m_Enabled = true;
            protected bool m_GettingInput = true;

            public InputAxis(KeyCode positive, KeyCode negative)
            {
                this.positive = positive;
                this.negative = negative;
            }

            public void Get(InputType inputType)
            {
                if (!m_Enabled)
                {
                    Value = 0f;
                    return;
                }

                if (!m_GettingInput)
                    return;

                bool positiveHeld = false;
                bool negativeHeld = false;

                
                if (inputType == InputType.MouseAndKeyboard)
                {
                    positiveHeld = Input.GetKey(positive);
                    negativeHeld = Input.GetKey(negative);
                }

                if (positiveHeld == negativeHeld)
                    Value = 0f;
                else if (positiveHeld)
                    Value = 1f;
                else
                    Value = -1f;

                ReceivingInput = positiveHeld || negativeHeld;
            }

            public void Enable()
            {
                m_Enabled = true;
            }

            public void Disable()
            {
                m_Enabled = false;
            }

            public void GainControl()
            {
                m_GettingInput = true;
            }

            public void ReleaseControl(bool resetValues)
            {
                m_GettingInput = false;
                if (resetValues)
                {
                    Value = 0f;
                    ReceivingInput = false;
                }
            }
        }

        public InputType inputType = InputType.MouseAndKeyboard;

        bool m_FixedUpdateHappened;

        void Update()
        {
            GetInputs(m_FixedUpdateHappened || Mathf.Approximately(Time.timeScale,0));

            m_FixedUpdateHappened = false;
        }

        void FixedUpdate()
        {
            m_FixedUpdateHappened = true;
        }

        protected abstract void GetInputs(bool fixedUpdateHappened);

        public abstract void GainControl();

        public abstract void ReleaseControl(bool resetValues = true);

        protected void GainControl(InputButton inputButton)
        {
            inputButton.GainControl();
        }

        protected void GainControl(InputAxis inputAxis)
        {
            inputAxis.GainControl();
        }

        protected void ReleaseControl(InputButton inputButton, bool resetValues)
        {
            StartCoroutine(inputButton.ReleaseControl(resetValues));
        }

        protected void ReleaseControl(InputAxis inputAxis, bool resetValues)
        {
            inputAxis.ReleaseControl(resetValues);
        }
    }
}