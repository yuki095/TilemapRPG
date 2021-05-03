/// <summary>
/// イベントの進行状態の種類
/// </summary>
public enum EventProgressType
{
    None,       // 会話イベントなし
    Need,       // ①アイテムを消費するタイプのイベントを発生させる
    Permission, // ②アイテムを持っているだけでOKのイベントを発生させる
    Get,        // ①か②をクリアしたときに発生する、アイテム獲得イベント
    Cleard      // イベントが終了した後の会話
}