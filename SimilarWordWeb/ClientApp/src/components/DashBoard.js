import React, { Component, Fragment, useState } from 'react';
import { Link } from 'react-router-dom';
import { Col, Grid, Row,Checkbox, Panel, PanelGroup, Carousel } from 'react-bootstrap';



const DashBoard = () => {

    const [memoryList, setMemoryList] = useState([]);
    const [firstFlag, setFirstFlag] = useState(0);
    const [memoryIdx, setMemoryIdx] = useState(0);

    const CallApi = () => {
        fetch('api/memory/')
            .then(response => response.json())
            .then(data => {
                console.log('fetch back');
                setMemoryList(data);
            });
    }
    const CallUpdateApi = (e) => {
        //alert("in CallUpdateApi");
        console.log("CallUpdateApi");
        e.preventDefault();
        fetch('api/memory/' +memoryList[memoryIdx].itemId, {
            method: 'PUT',
            body: '2',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => { console.log(response); setMemoryIdx(memoryIdx + 1) })
    }
    if (firstFlag == 0) {
        CallApi();
        setFirstFlag(1);
    }

    if (memoryList.length <= 0) return 'no items to review!';

    if (memoryIdx >= memoryList.length) return 'Congraduation, you finished: ' + memoryList.length;
    console.log('memoryList.Length=' + memoryList.length.toString()+',idx='+memoryIdx);
    console.log(memoryList[memoryIdx]);
        return (
                <Panel bsStyle="primary">
                    <Panel.Heading>
                    <Panel.Title componentClass="h3">
                        {memoryList[memoryIdx].itemId}{' (' + (memoryIdx+1).toString() + '/' + memoryList.length.toString()+')'}
                    </Panel.Title>
                    </Panel.Heading>
                    <Panel.Body>
                        <span>{' Dash Board '}</span>
                    <br />{memoryList[memoryIdx].viewTime}<br />{memoryList[memoryIdx].viewInterval}
                    <Link to={'/wordsearch/' + memoryList[memoryIdx].itemId} > Show Detail</Link><br />

                    <button onClick={CallUpdateApi}>{'Review in ' + memoryList[memoryIdx].viewInterval + 'days'}</button>
                    </Panel.Body>
                </Panel>
        );
}
        
        
export default DashBoard;

