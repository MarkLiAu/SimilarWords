import React, { PureComponent } from 'react';
import {
    BarChart, Bar, Brush, ReferenceLine, XAxis, YAxis, CartesianGrid, Tooltip, Legend,
} from 'recharts';

export default class Example extends PureComponent {
                //<Bar dataKey="newViewed" fill="#82ca9d" />

    render() {
        if (this.props.data.length > 0)
            return (
                <BarChart
                    width={500}
                    height={300}
                    data={this.props.data}
                    margin={{
                        top: 5, right: 30, left: 20, bottom: 5,
                    }}
                >
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="viewTime" />
                    <YAxis />
                    <Tooltip />
                    <Legend verticalAlign="top" wrapperStyle={{ lineHeight: '40px' }} />
                    <ReferenceLine y={0} stroke="#000" />
                    <Brush dataKey="viewTime" height={30} stroke="#8884d8" />
                    <Bar dataKey="totalViewed" fill="#8884d8" />
                </BarChart>
            );
        else return null;
    }
}
