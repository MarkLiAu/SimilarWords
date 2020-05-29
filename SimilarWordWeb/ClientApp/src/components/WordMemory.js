import React, { Component, Fragment, useState } from 'react';
import { Link } from 'react-router-dom';
import { Col, Grid, Row,Checkbox, Panel, PanelGroup, Carousel } from 'react-bootstrap';
import WordInfoDisplay from './WordInfoDisplay';


const WordMemory = () => {

    const [memoryList, setMemoryList] = useState([]);
    const [firstFlag, setFirstFlag] = useState(0);
    const [memoryIdx, setMemoryIdx] = useState(0);
    const [showDetail, setShowDetail] = useState(false);
    const [curWord, setCurWord] = useState({easiness:0,viewInterval:0});

    const CallApi = () => {
        fetch('api/memory/')
            .then(response => response.json())
            .then(data => {
                console.log('fetch back');
                setMemoryList(data);
                ResetCurWord();
            });
    }
    const CallUpdateApi = (e) => {
        //alert("in CallUpdateApi");
        console.log("CallUpdateApi");
        e.preventDefault();
        memoryList[memoryIdx].easiness = curWord.easiness;
        memoryList[memoryIdx].viewInterval = curWord.viewInterval;
        setMemoryList(memoryList);
        fetch('api/memory/' +memoryList[memoryIdx].name, {
            method: 'PUT',
            body: JSON.stringify(memoryList[memoryIdx]),
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(response => {
            console.log(response);
            memoryList[memoryIdx].easiness = 999;
            if (memoryIdx < memoryList.length - 1) setMemoryIdx(memoryIdx + 1);
            ResetCurWord();
            setShowDetail(false);
        })
    }

    const ResetCurWord = () => {
        if (memoryList.length > 0) {
            curWord.easiness = memoryList[memoryIdx].easiness;
            if (curWord.easiness < -2 || curWord.easiness > 2) curWord.easiness = 0;
            curWord.viewInterval = memoryList[memoryIdx].viewInterval;
            if (curWord.viewInterval < 0) curWord.viewInterval = 0;
            setCurWord(curWord);
        }
    }

    const ChangeEasiness = (e, increase) => {
        //let increase = e.srcElement.id == 'btn_hard' ? 1 : -1;
        console.log('in changeeasiness');
        console.log(increase);
        console.log(curWord);
        let v = Math.round(curWord.viewInterval * 0.1,0);
        if (v < 1) v = 1;
        curWord.viewInterval += v*increase;
        if (curWord.viewInterval <= 0) curWord.viewInterval = 0;
        if (increase > 0 && curWord.viewInterval <= 0) curWord.viewInterval = 1;

        if (curWord.viewInterval > memoryList[memoryIdx].viewInterval) curWord.easiness = 1;
        else if (curWord.viewInterval < memoryList[memoryIdx].viewInterval) curWord.easiness = -1;
        else curWord.easiness = 0;

        setCurWord(curWord);
        setFirstFlag(firstFlag + 1);
        console.log(curWord);
    }

    const showDetailClicked = () => { setShowDetail(!showDetail)}

    const backClicked = () => {
        if (memoryIdx > 0) {
            setShowDetail(false);
            setMemoryIdx(memoryIdx - 1);
            ResetCurWord();
        }
    }
    const nextClicked = () => {
        if (memoryIdx < memoryList.length - 1) {
            setShowDetail(false);
            setMemoryIdx(memoryIdx + 1);
            ResetCurWord();
         }
    }

    function reviewDays(d) { return ( d <= 0 ? 'Review Today' : 'Review in ' + d + ' day' + (d < 1 ? 's' : '')) ; } 

    if (firstFlag == 0) {
        CallApi();
        setFirstFlag(1);
    }

    if (memoryList.length <= 0) return 'no items to review!';

    let nwaiting = 0; memoryList.map(x =>nwaiting+=(x.easiness < 999? 1:0));
    if (nwaiting===0) return 'Congraduation, you finished: ' + memoryList.length;

    console.log('memoryList.Length=' + memoryList.length.toString()+',idx='+memoryIdx);
    console.log(memoryList[memoryIdx]);
        return (
                <Panel bsStyle="primary">
                    <Panel.Heading>
                    <Panel.Title componentClass="h3">
                        {memoryList[memoryIdx].name}{' (' + (memoryIdx+1).toString() + '/' + memoryList.length.toString()+')'}
                        {'   '}<button className='btn btn-info' id='btn_submit' disabled={memoryList[memoryIdx].easiness >= 999} onClick={CallUpdateApi}>{reviewDays(curWord.viewInterval)}</button>
                    </Panel.Title>
                    </Panel.Heading>
                    <Panel.Body>

                   <button className='btn btn-danger' id='btn_hard' disabled={memoryList[memoryIdx].easiness >=999} onClick={(e)=>ChangeEasiness(e,-1)}>Hard</button>{' '}
                    <button className='btn btn-success' id='btn_easy' disabled={memoryList[memoryIdx].easiness >=999} onClick={(e) => ChangeEasiness(e, 1)}>Easy</button>
                    {'      '} 
                    <button className='btn' id='btn_back' onClick={backClicked}>{'<Back'}</button>
                    {' '} 
                    <button className='btn'  id='btn_next' onClick={nextClicked}>{'Next>'}</button>
                    {' '}
                        <button className='btn' id='btn_detail' onClick={showDetailClicked}>{'Detail...'}</button>
                    
                    </Panel.Body>
                    <WordInfoDisplay word={showDetail?memoryList[memoryIdx]:null}></WordInfoDisplay>
                </Panel>
        );
}
        
        
export default WordMemory;

