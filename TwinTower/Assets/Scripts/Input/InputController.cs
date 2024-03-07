using System.Collections.Generic;
using UnityEngine;


namespace TwinTower
{
    public class InputController : InputComponent<InputController>
    {

        protected bool _HaveControl = true;

        protected bool _DebugMenuIsOpen = false;
        // <키들을 정의하는 공간입니다>
        public InputButton LeftMove;
        public InputButton RightMove;
        public InputButton UpMove;
        public InputButton DownMove;
        public InputButton ResetButton;
        
        public InputAxis Horizontal;
        public InputAxis Vertical;
        // </키들을 정의하는 공간입니다>
        
        protected override void Awake()
        {
            base.Awake();

            LeftMove = new InputButton(KeyCode.LeftArrow);
            RightMove = new InputButton(KeyCode.RightArrow);
            UpMove = new InputButton(KeyCode.UpArrow);
            DownMove = new InputButton(KeyCode.DownArrow);
            ResetButton = new InputButton(KeyCode.R);
            
            Horizontal = new InputAxis(KeyCode.D, KeyCode.A);
            Vertical = new InputAxis(KeyCode.W,KeyCode.S);
        }

        /// <summary>
        /// 입력을 가져오는 함수입니다.
        /// </summary>
        /// <param name="fixedUpdateHappened"></param>
        protected override void GetInputs(bool fixedUpdateHappened)
        {
            _HaveControl = true;
            
            LeftMove.Get(fixedUpdateHappened, inputType);
            RightMove.Get(fixedUpdateHappened, inputType);
            UpMove.Get(fixedUpdateHappened, inputType);
            DownMove.Get(fixedUpdateHappened, inputType);
            ResetButton.Get(fixedUpdateHappened, inputType);
            
            Horizontal.Get(inputType);
            Vertical.Get(inputType);
        }
        /// <summary>
        /// 막은 키 입력을 다시 활성화 합니다.
        /// </summary>
        /// <param name="resetValues"></param>

        public override void GainControl()
        {
            _HaveControl = true;
            
            GainControl(LeftMove);
            GainControl(RightMove);
            GainControl(UpMove);
            GainControl(DownMove);
            GainControl(ResetButton);
            
            GainControl(Horizontal);
            GainControl(Vertical);
           }
        
        /// <summary>
        /// 플레이어의 키 입력을 막습니다.
        /// </summary>
        /// <param name="resetValues"></param>

        public override void ReleaseControl(bool resetValues = true)
        {
            _HaveControl = false;
            
            ReleaseControl(Horizontal,resetValues);
            ReleaseControl(Vertical,resetValues);
        }
    }
}