@echo off
title ��װ֤��
color 0A
cd cert

:begin
Echo ˵����֤�������ƹ� UAC ����
Echo.
Echo ��ʾ��װ֤��ʱ ��ѡ���ǣ�Y����
Echo.
Echo ---------------------------------
Echo I   1 �̻�                      I
Echo I   2 ж��                      I
Echo ---------------------------------
Set /P var=����ѡ����س���ִ�У�

If not "%var%"=="" (
  If "%var%"=="1"  goto �̻�
  If "%var%"=="2"  goto ж��
)

goto :begin

:�̻�
CertMgr -add -c YingDevCA.cer -s root
goto exit

:ж��
CertMgr -del -c -n "WGestures Test CA" -s root
goto exit

:exit
rem pause