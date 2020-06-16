import React, { Component, Fragment, useState } from 'react';
import { Link } from 'react-router-dom';
import { Col, Grid, Row, Checkbox, Panel, PanelGroup, Table } from 'react-bootstrap';
import { GetTokenHeader } from './CommTools';
import { CSVLink } from 'react-csv'
import StandardTable from './StandardTable';


export const MemoryLog = ({ cmd })=> {
    const [userInput, setUserInput] = useState('10');
    const [memLog, setMemLog] = useState([]);

    const InputChanged = (e) => {
        setUserInput(e.target.value);
    }

    const SearchLog = () => {
        console.log('memorylog:' + userInput);
        if (userInput.length <= 0) return;
        fetch('api/Memory/' + userInput, {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': GetTokenHeader()
            }
        })
            .then(response => response.json())
            .then(data => {
                console.log('fetch back');
                setMemLog(data.map(d => { d.viewTime = new Date(d.viewTime).toLocaleString(); return d; }));
            });
    }

    const DownloadLog = () => {
        console.log('memorylog-DownloadLog:' + userInput);
        if (userInput.length <= 0) return;
        fetch('api/Memory/Csv', {
            method: 'POST',
            body: userInput,
            headers: {
                'Content-Type': 'application/json',
                'Authorization': GetTokenHeader()
            }
        })
            .then(response => {
                console.log('DownloadLog fetch back:'+response.ok);
            });
    }


    const KeyPressed = (event) => {
        if (event.key === 'Enter') {
            SearchLog();
        }
    }

    const ShowLog__NOTUSED = ({ log }) => {
        return (
            log.map((d, idx) => <tr key={idx}>
                <td>{new Date(d.viewTime).toLocaleString()}</td>
                <td>{d.name}</td>
                <td>{d.easiness} {d.viewTime}{'!'}</td>
                <td>{d.viewInterval}</td>
            </tr>
            )
            )
    }

    const columns = [
        {
            Header: "Memory Log",
            columns: [
                {
                    Header: "Time",
                    accessor: "viewTime",
                    sortType: "basic"
                },
                {
                    Header: "name",
                    accessor: "name",
                    sortType: "basic"
                },
                {
                    Header: "Easiness",
                    accessor: "easiness",
                    sortType: "basic"
                },
                {
                    Header: "Interval",
                    accessor: "viewInterval",
                    sortType: "basic"
                }
            ]
        }
    ];


    console.log('MemoryLog render');
    console.log(memLog);
    return (
        <div className='bj_center'>
                Days to load:<input defaultValue={userInput} placeholder='days back' onChange={InputChanged} onKeyPress={KeyPressed} ></input>
            word to load:<input  placeholder='word' onKeyPress={KeyPressed} ></input>
                {' '}
            <button type="submit" onClick={SearchLog}>Show</button>
            {' '}
            <CSVLink className='button'
                data={memLog}
                filename="data.csv"
                    target="_blank" >
                    <button>Download</button>
                </CSVLink>
            <StandardTable data={memLog} columns={columns} ></StandardTable>

        </div>
    );
}

