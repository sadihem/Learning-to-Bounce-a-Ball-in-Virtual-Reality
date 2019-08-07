


fileID = fopen( 'optimalAct.csv' , 'w' );
state1num= 7 ;
state2num= 11 ;
state3num= 7 ;
for state1= 1 :state1num- 1 %doesnt print failure states
for state2= 1 :state2num
for state3= 1 :state3num- 1
actionsForGivenState = Q(:,:,:,state1, state2, state3);
% 1 dementionalize and find max
M = actionsForGivenState;%= randn( 10 , 10 , 10 , 10 );
[C,I] = max(M(:));
temp = find(max(M(:)) == M(:));
[C,I] = max(M(:));
%C = C;
randI = temp(randi([ 1 length (temp)]));
M(randI);
[I1,I2,I3] = ind2sub( size (M),randI);
M(I1,I2,I3);
maxAction1 = I1;
maxAction2 = I2;
maxAction3 = I3;
if ( Q (maxAction1,maxAction2,maxAction3,state1, state2, state3) <= 0 )
% fprintf (fileID, '%i,%i,%i V:%d
State:%i,%i,%i \n ' ,maxAction1,maxAction2,maxAction3, Q (maxAction1,maxAction2,maxAction3,state1, state2,
state3),state1,state2,state3);
fprintf(fileID, '%i,%i,%i,%d,%i,%i,%i \n ' ,- 1 ,- 1 ,- 1 , Q (maxAction1,maxAction2,maxAction3,state1,
state2, state3),state1,state2,state3);
else
fprintf(fileID, '%i,%i,%i,%d,%i,%i,%i \n ' ,maxAction1,maxAction2,maxAction3, Q (maxAction1,maxAction2,maxAction
3,state1, state2, state3),state1,state2,state3);
end
end
end
end
