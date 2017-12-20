import cv2

def frame_resize(frame, n=2):
    """
    スクリーンショットを撮りたい関係で1/4サイズに縮小
    """
    return cv2.resize(frame, (int(frame.shape[1]/2), int(frame.shape[0]/2)))

if __name__ == '__main__':
    """
    Tracking手法を
    """
    # Boosting
    # tracker = cv2.TrackerBoosting_create()

    # MIL
    # tracker = cv2.TrackerMIL_create()

    # KCF
    # tracker = cv2.TrackerKCF_create()

    # TLD #GPUコンパイラのエラーが出ているっぽい
    tracker = cv2.TrackerTLD_create()

    # MedianFlow
    # tracker = cv2.TrackerMedianFlow_create()

    # GOTURN # モデルが無いよって怒られた
    # https://github.com/opencv/opencv_contrib/issues/941#issuecomment-343384500
    # tracker = cv2.TrackerGOTURN_create()

    cap = cv2.VideoCapture(0)

    while True:
        ret, frame = cap.read()
        if not ret:
            continue
        frame = frame_resize(frame)
        bbox = (0,0,10,10)
        bbox = cv2.selectROI(frame, False)
        ok = tracker.init(frame, bbox)
        cv2.destroyAllWindows()
        break

    while True:
        # VideoCaptureから1フレーム読み込む
        ret, frame = cap.read()
        frame = frame_resize(frame)
        if not ret:
            k = cv2.waitKey(1)
            if k == 27 :
                break
            continue

        # Start timer
        timer = cv2.getTickCount()

        # トラッカーをアップデートする
        track, bbox = tracker.update(frame)

        # FPSを計算する
        fps = cv2.getTickFrequency() / (cv2.getTickCount() - timer);

        # 検出した場所に四角を書く
        if track:
            # Tracking success
            p1 = (int(bbox[0]), int(bbox[1]))
            p2 = (int(bbox[0] + bbox[2]), int(bbox[1] + bbox[3]))
            cv2.rectangle(frame, p1, p2, (0,255,0), 2, 1)
        else :
            # トラッキングが外れたら警告を表示する
            cv2.putText(frame, "Failure", (10,50), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0,255,0), 1, cv2.LINE_AA);

        # FPSを表示する
        cv2.putText(frame, "FPS : " + str(int(fps)), (10,20), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0,255,0), 1, cv2.LINE_AA);

        # 加工済の画像を表示する
        cv2.imshow("Tracking", frame)

        # キー入力を1ms待って、k が27（ESC）だったらBreakする
        k = cv2.waitKey(1)
        if k == 27 :
            break

# キャプチャをリリースして、ウィンドウをすべて閉じる
cap.release()
cv2.destroyAllWindows()
