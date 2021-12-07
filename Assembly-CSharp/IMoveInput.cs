using System;
using FixedPoint;

// Token: 0x02000524 RID: 1316
public interface IMoveInput
{
	// Token: 0x06001C78 RID: 7288
	Fixed GetHorizontalAxis();

	// Token: 0x06001C79 RID: 7289
	Fixed GetVerticalAxis();

	// Token: 0x06001C7A RID: 7290
	bool GetButton(ButtonPress button);

	// Token: 0x06001C7B RID: 7291
	bool GetButtonDown(ButtonPress button);

	// Token: 0x06001C7C RID: 7292
	Vector2F GetAxisValue();

	// Token: 0x06001C7D RID: 7293
	string GenerateDebugString();
}
