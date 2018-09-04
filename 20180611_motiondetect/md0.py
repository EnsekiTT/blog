import cv2

cap = cv2.VideoCapture(0)

ok = False
before = None
detected_frame = None
bbox = (0,0,0,0)
while True:
    #  OpenCVでWebカメラの画像を取り込む
    ret, frame = cap.read()

    # スクリーンショットを撮りたい関係で1/2サイズに縮小
    frame = cv2.resize(frame, (int(frame.shape[1]/2), int(frame.shape[0]/2)))
    # 加工なし画像を表示する
    cv2.imshow('Raw Frame', frame)

    # 取り込んだフレームに対して差分をとって動いているところが明るい画像を作る
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    if before is None:
        before = gray.copy().astype('float')
        continue
    cv2.accumulateWeighted(gray, before, 0.7)
    mdframe = cv2.absdiff(gray, cv2.convertScaleAbs(before))
    # 動いているところが明るい画像を表示する
    # cv2.imshow('MotionDetected Frame', mdframe)

    # 動いているエリアの面積を計算してちょうどいい検出結果を抽出する
    thresh = cv2.threshold(mdframe, 3, 255, cv2.THRESH_BINARY)[1]
    image, contours, hierarchy = cv2.findContours(thresh.copy(), cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    max_area = 0
    target = None
    for cnt in contours:
        area = cv2.contourArea(cnt)
        if max_area < area and area < 40000 and area > 4000:
            max_area = area;
            target = cnt

    # 動いているエリアのうちそこそこの大きさのものがあればそれを矩形で表示する
    # ちょうどいいエリアがなかったら最後の動いているエリアがあるフレームとエリア情報を用いてトラッキングをする
    # どうしようもない時はどうしようもない旨を表示する
    if max_area <= 4000:
        track = False
        if detected_frame is not None:
            # インスタンスを作り直さなきゃいけないっぽい
            tracker = cv2.TrackerKCF_create()
            ok = tracker.init(detected_frame, bbox)
            detected_frame = None

        if ok:
            track, bbox = tracker.update(frame)
        if track:
            p1 = (int(bbox[0]), int(bbox[1]))
            p2 = (int(bbox[0] + bbox[2]), int(bbox[1] + bbox[3]))
            cv2.rectangle(frame, p1, p2, (0,255,0), 2, 1)
            cv2.putText(frame, "tracking", (10,50), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0,255,0), 1, cv2.LINE_AA)
        else:
            ok = False
            cv2.putText(frame, "(^q^)", (10,50), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0,255,0), 1, cv2.LINE_AA)
    else:
        #areaframe = cv2.drawContours(frame, [target], 0, (0,255,0), 3)
        x,y,w,h = cv2.boundingRect(target)
        bbox = (x,y,w,h)
        detected_frame = frame.copy()
        frame = cv2.rectangle(frame,(x,y),(x+w,y+h),(0,255,0),2)
        cv2.putText(frame, "motion detected", (10,50), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0,255,0), 1, cv2.LINE_AA)

    cv2.imshow('MotionDetected Area Frame', frame)
    # キー入力を1ms待って、k が27（ESC）だったらBreakする
    k = cv2.waitKey(1)
    if k == 27:
        break

# キャプチャをリリースして、ウィンドウをすべて閉じる
cap.release()
cv2.destroyAllWindows()
