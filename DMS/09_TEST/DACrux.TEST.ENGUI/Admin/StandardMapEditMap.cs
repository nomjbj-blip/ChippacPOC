using DACrux.Base;

namespace DACrux.TEST.ENGUI
{
    /// <summary>
    /// File Map Setup 화면 전용 Map 컨트롤. 기존 EditWaferMap 을 그대로 상속하고 원 안 판정 함수 하나만 밖으로 연다.
    /// 왜: WaferMap.UseDie (Wafer 반지름 - Edge / Flat·Notch 영역 판정) 는 protected 라 화면에서 부를 수 없다.
    ///     06_CoreLibSource 는 수정하지 않으므로 상속해서 같은 판정을 재사용한다.
    /// 주의: WaferMap.DieCalculation(true) 는 LeftBottom 등에서 AddDie 와 Die 위치 계산이 한 칸 다르다.
    ///     그래서 이 화면은 DieCalculation 대신 AddDie 로 격자를 넣고, 이 함수로 Die 마다 원 안 여부를 정한다.
    /// </summary>
    public class StandardMapEditMap : DACrux.Map.EditWaferMap
    {
        /// <summary>
        /// Die 가 Wafer 안(반지름 - Edge, Notch / Flat 영역 밖)에 완전히 들어가는지 판정한다.
        /// DieCood 는 AddDie 가 계산한 Die 좌하단 좌표(원점 기준)이고, UseDie 는 Wafer 중심 기준 좌표를 받으므로 Origin 을 뺀다.
        /// Notch / Flat 영역은 그리기 중에 계산되므로 설정을 바꾼 뒤 Redraw 를 먼저 한 번 해야 한다.
        /// </summary>
        public bool IsInsideWafer(Die die)
        {
            return UseDie(die.DieCood.X - OriginX, die.DieCood.Y - OriginY, die.DieCood.Width, die.DieCood.Height);
        }
    }
}
