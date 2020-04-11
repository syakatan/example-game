// This script handles inputs for the player. It serves two main purposes: 1) wrap up
// inputs so swapping between mobile and standalone is simpler and 2) keeping inputs
// from Update() in sync with FixedUpdate()
//このスクリプトは、プレーヤーの入力を処理します。主な目的は2つあります。
//1）入力をラップアップしてモバイルとスタンドアロンの切り替えを簡単にする、
//2）Updateからの入力をFixedUpdateと同期する（）

using UnityEngine;

//We first ensure this script runs before all other player scripts to prevent laggy
//inputs
//遅延を防ぐために、他のすべてのプレーヤースクリプトの前にこのスクリプトが実行されることを最初に確認します
//入力
[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
	public bool testTouchControlsInEditor = false;  //Should touch controls be tested?//タッチコントロールをテストする必要がありますか？
	public float verticalDPadThreshold = .5f;       //Threshold touch pad inputs//しきい値タッチパッド入力
	public Thumbstick thumbstick;                   //Reference to Thumbstick//サムスティックへの参照
	public TouchButton jumpButton;                  //Reference to jump TouchButton//ジャンプタッチボタンへの参照

	[HideInInspector] public float horizontal;      //Float that stores horizontal input//水平入力を保存するフロート
	[HideInInspector] public bool jumpHeld;         //Bool that stores jump pressed//押されたジャンプを格納するBool
	[HideInInspector] public bool jumpPressed;      //Bool that stores jump held//ジャンプを保存するBoolを保持
	[HideInInspector] public bool crouchHeld;       //Bool that stores crouch pressed//押されたしゃがみを保存するBool
	[HideInInspector] public bool crouchPressed;    //Bool that stores crouch held//クラウチを保持するBool

	bool dPadCrouchPrev;                            //Previous values of touch Thumbstick//タッチサムスティックの以前の値
	bool readyToClear;                              //Bool used to keep input in sync//入力の同期を保つために使用されるBool


	void Update()
	{
		//Clear out existing input values//既存の入力値を消去します
		ClearInput();

		//If the Game Manager says the game is over, exit
		//ゲームマネージャーがゲームオーバーであると言った場合、終了
		if (GameManager.IsGameOver())
			return;

		//Process keyboard, mouse, gamepad (etc) inputs
		//キーボード、マウス、ゲームパッドなどの入力を処理します
		ProcessInputs();
		//Process mobile (touch) inputs
		//モバイル（タッチ）入力を処理します
		ProcessTouchInputs();

		//Clamp the horizontal input to be between -1 and 1
		//水平入力を-1〜1にクランプします
		horizontal = Mathf.Clamp(horizontal, -1f, 1f);
	}

	void FixedUpdate()
	{
		//In FixedUpdate() we set a flag that lets inputs to be cleared out during the 
		//next Update(). This ensures that all code gets to use the current inputs
		// FixedUpdate（）で、入力中に入力をクリアできるフラグを設定します
		// next Update（）。これにより、すべてのコードが現在の入力を使用するようになります
		readyToClear = true;
	}

	void ClearInput()
	{
		//If we're not ready to clear input, exit
		//入力をクリアする準備ができていない場合は、終了します
		if (!readyToClear)
			return;

		//Reset all inputs//すべての入力をリセット
		horizontal = 0f;
		jumpPressed		= false;
		jumpHeld		= false;
		crouchPressed	= false;
		crouchHeld		= false;

		readyToClear	= false;
	}

	void ProcessInputs()
	{
		//Accumulate horizontal axis input
		//水平軸入力を累積します
		horizontal += Input.GetAxis("Horizontal");

		//Accumulate button inputs
		//ボタン入力を累積します
		jumpPressed = jumpPressed || Input.GetButtonDown("Jump");
		jumpHeld		= jumpHeld || Input.GetButton("Jump");

		crouchPressed	= crouchPressed || Input.GetButtonDown("Crouch");
		crouchHeld		= crouchHeld || Input.GetButton("Crouch");
	}

	void ProcessTouchInputs()
	{
		//If this isn't a mobile platform AND we aren't testing in editor, exit
		//これがモバイルプラットフォームではなく、エディターでテストしていない場合は、終了します
		if (!Application.isMobilePlatform && !testTouchControlsInEditor)
			return;

		//Record inputs from screen thumbstick
		//画面のサムスティックからの入力を記録します
		Vector2 thumbstickInput = thumbstick.GetDirection();

		//Accumulate horizontal input
		//水平入力を累積します
		horizontal += thumbstickInput.x;

		//Accumulate jump button input
		//ジャンプボタン入力を累積する
		jumpPressed = jumpPressed || jumpButton.GetButtonDown();
		jumpHeld		= jumpHeld || jumpButton.GetButton();

		//Using thumbstick, accumulate crouch input
		//サムスティックを使用して、しゃがみ入力を蓄積します
		bool dPadCrouch = thumbstickInput.y <= -verticalDPadThreshold;
		crouchPressed	= crouchPressed || (dPadCrouch && !dPadCrouchPrev);
		crouchHeld		= crouchHeld || dPadCrouch;

		//Record whether or not playing is crouching this frame (used for determining
		//if button is pressed for first time or held
		//再生がこのフレームをしゃがんでいるかどうかを記録します（決定に使用されます
		//ボタンが初めて押された場合、または保持された場合
		dPadCrouchPrev = dPadCrouch;
	}
}
