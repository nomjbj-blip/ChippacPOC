using NexplantQMS.GdsMap.Chain;
using NexplantQMS.GdsMap.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS Map View 화면의 Front / Back side 전환 중 Chain 쪽 좌표 처리다. (설계: 문서/설계/02_GDS/2026-10-07_GDS_FrontBack_전환_설계.md 4 / 5.2절)
    ///
    /// 좌표 공간 규칙
    ///  - DB(PLACED / Chain Marker)는 저장 기준(TQP_GDS_MAP.VIEW_SIDE) 좌표. 화면은 보기 기준(_displaySide) 좌표.
    ///  - 두 기준이 다르면 X' = -X(GdsSideTransform). Chain 소속은 도형 ID라서 기준과 무관하다.
    ///  - Chain 저장 행 / 저장 지문은 항상 저장 기준 좌표로 만든다(ToStoredX). 그래서 보기 기준만 바꿔도 "변경 없음"이 유지된다.
    ///  - 보기 기준을 바꿀 때 Chain의 Marker / 단자 후보 / 탐색 결과 좌표 사본은 ID로 다시 찾지 않고 좌표만 반전한다.
    ///    조회하지 않은 Layer 도형도 탐색 결과에 남아 있어야 저장 때 Member가 빠지지 않기 때문이다.
    /// </summary>
    public partial class GdsMapViewForm
    {
        /// <summary>지금 화면이 저장 기준과 다른 기준(반전)으로 그려져 있는지.</summary>
        private bool DisplayMirrored
        {
            get { return _shownMap != null && GdsSideTransform.NeedsMirror(_shownMap.ViewSide, _displaySide); }
        }

        /// <summary>화면 X 좌표 -> 저장 기준 X 좌표(Chain 저장 / 저장 지문용).</summary>
        private double ToStoredX(double displayX)
        {
            return DisplayMirrored ? GdsSideTransform.MirrorX(displayX) : displayX;
        }

        /// <summary>저장 기준 X 좌표 -> 화면 X 좌표(Chain 불러오기용). 변환이 부호 반전이라 ToStoredX와 같은 식이다.</summary>
        private double ToDisplayX(double storedX)
        {
            return DisplayMirrored ? GdsSideTransform.MirrorX(storedX) : storedX;
        }

        /// <summary>
        /// 보기 기준이 바뀐 직후(지도는 이미 새 기준으로 다시 그림) 모든 Chain의 화면 좌표 사본을 좌우 반전한다.
        /// 대상: Input / Output Marker 좌표와 단자 후보 Bounds, 탐색 결과(LastResult)의 모든 도형 좌표 / Bounds.
        /// 호출 전 CaptureActiveChain으로 편집 중 상태를 Chain 정의에 보관하고, 호출 뒤 ReattachChainsAfterMapReload로 화면에 다시 연결한다.
        /// </summary>
        private void MirrorChainsForSideChange()
        {
            foreach (ChainDefinition chain in _chains)
            {
                chain.Inputs = MirrorPicks(chain.Inputs);
                chain.Outputs = MirrorPicks(chain.Outputs);
                if (chain.LastResult != null)
                    chain.LastResult = MirrorTraceResult(chain.LastResult);
            }
        }

        private static Dictionary<string, ChainEndpointPick> MirrorPicks(Dictionary<string, ChainEndpointPick> picks)
        {
            var result = new Dictionary<string, ChainEndpointPick>(StringComparer.Ordinal);
            foreach (KeyValuePair<string, ChainEndpointPick> pair in picks)
                result[pair.Key] = new ChainEndpointPick
                {
                    Candidate = MirrorCandidate(pair.Value.Candidate),
                    Point = GdsSideTransform.MirrorPoint(pair.Value.Point)
                };
            return result;
        }

        private static ChainElementCandidate MirrorCandidate(ChainElementCandidate c)
        {
            if (c == null) return null;
            return new ChainElementCandidate(c.ElementKey, c.LayerId, c.DataType, c.ElementType,
                GdsSideTransform.MirrorBounds(c.Bounds), c.PointCount, c.PathWidth);
        }

        /// <summary>탐색 결과의 구성(경로 / 방문 / 겹침 / 분기 / 단자 / 열린 탐색 여부 / 메시지)은 그대로 두고 도형 좌표만 반전한 새 결과.</summary>
        private static ChainTraceResult MirrorTraceResult(ChainTraceResult r)
        {
            return new ChainTraceResult(r.IsConnected, MirrorElements(r.Path), MirrorElements(r.VisitedElements), r.Message,
                r.InputLayerId, r.OutputLayerId, MirrorElements(r.OverlappingElements), MirrorElements(r.BranchCandidates),
                MirrorElements(r.EndpointElements), r.IsOpenEnded);
        }

        private static List<ChainTraceElement> MirrorElements(IList<ChainTraceElement> elements)
        {
            return elements.Select(e => new ChainTraceElement(e.ElementKey, e.LayerId, GdsSideTransform.MirrorBounds(e.Bounds), e.ElementType,
                e.WorldPoints == null ? null : GdsSideTransform.MirrorPoints(e.WorldPoints), e.PathWidth, e.DataType)).ToList();
        }
    }
}
