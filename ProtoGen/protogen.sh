filelist=`ls *.proto`  
echo $filelist
for file in $filelist  
do  
echo $file  
protogen -i:$file -o:'../Assets/Script/Net/Proto/'${file%.*}.cs  
done
