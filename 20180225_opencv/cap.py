import cv2
"""Capture video from camera"""
# カメラをキャプチャする
cap = cv2.VideoCapture(0)
ret, frame = cap.read()
height, width = frame.shape[:2]
fps = 30
# 出力先のファイルを開く
fourcc = cv2.VideoWriter_fourcc(*'mp4v')
out = cv2.VideoWriter('output.mp4', int(fourcc), fps, (int(width), int(height)))

while True:
    ret, frame = cap.read()
    cv2.imshow('camera capture', frame)

    out.write(frame)
    k = cv2.waitKey(10) # 10msec待つ
    if k == 27: # ESCキーで終了
        break

# 解放する
cap.release()
out.release()
cv2.destroyAllWindows()
