using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ChuCong : BaseMeshEffect
{
    public float curveStrength = 0.5f;

    public override void ModifyMesh(VertexHelper vh)
    {
        // if (!IsActive()) return;

        var verts = new UIVertex[4];
        int vertCount = vh.currentVertCount;

        for (int i = 0; i < vertCount; i += 4)
        {
            for (int j = 0; j < 4; j++)
            {
                vh.PopulateUIVertex(ref verts[j], i + j);
            }

            float midX = (verts[0].position.x + verts[2].position.x) / 2f;

            for (int j = 0; j < 4; j++)
            {
                Vector3 position = verts[j].position;
                float xOffset = position.x - midX;
                float yOffset = -curveStrength * xOffset * xOffset;
                position.y += yOffset;
                verts[j].position = position;
                vh.SetUIVertex(verts[j], i + j);
            }
        }
    }
}